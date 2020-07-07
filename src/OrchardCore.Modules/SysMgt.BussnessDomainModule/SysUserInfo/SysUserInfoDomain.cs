using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using SysMgt.BussnessDomainModule.Common;
using SysMgt.BussnessDomainModule.ProductElementType;
using SysMgt.BussnessDTOModule;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.SysUserInfo;
using SysMgt.BussnessRespositoryModule;
using SysMgt.BussnessDTOModule.SysRoleInfo;
using SysMgt.BussnessDomainModule.SysRoleUser;
using BlocksCore.Abstractions.Security;

namespace SysMgt.BussnessDomainModule.SysUserInfo
{
    public class SysUserInfoDomain : IDomainService
    {
        /// <summary>
        /// 申明接口
        /// </summary>
        private ISysUserInfoRepository SysUserInfoRepository { get; set; }

        public ISysRoleUserRepository SysRoleUserRepository { get; set; }
        public ISysRoleInfoRepository SysRoleInfoRepository { get; set; }
        private IUserContext UserContext;
        public IStringLocalizer L { get; set; }
        

        /// <summary>
        /// 构造函数,实例化对象
        /// </summary>
        /// <param name="sysUserInfoRepository"></param>
        public SysUserInfoDomain(ISysUserInfoRepository sysUserInfoRepository, ISysRoleUserRepository sysRoleUserRepository, ISysRoleInfoRepository sysRoleInfoRepository,IUserContext userContext)
        {
            this.SysUserInfoRepository = sysUserInfoRepository;
            this.SysRoleUserRepository = sysRoleUserRepository;
            this.SysRoleInfoRepository = sysRoleInfoRepository;
            this.UserContext = userContext;
        }


        public virtual SysUserInfoData GetOneById(SysUserInfoData sysUserInfoData)
        {
            var sysUserInfo = SysUserInfoRepository.FirstOrDefault(t => t.Id == sysUserInfoData.ID);
            if (sysUserInfo == null)
            {
                throw new BlocksBussnessException("101", L["未查到对象"], null);
            }
            return new SysUserInfoData()
            {
                ID = sysUserInfo.Id,
                UserCode = sysUserInfo.USERCODE,
                CName = sysUserInfo.CNAME,
                Password = sysUserInfo.PASSWORD,
                State = sysUserInfo.STATE,
                Memo = sysUserInfo.MEMO,
            };
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public virtual PageList<SysUserInfoPageResult> GetPageList(SysUserInfoSearchModel search)
        {
            var list =  SysUserInfoRepository.GetPageList(search); 
            foreach (SysUserInfoPageResult item in list.Rows)
            { 
                var roleUserList = SysRoleUserRepository.GetAllList(t => t.SYS_USERINFOID == item.ID);
                List<string> roleList = new List<string>();
                foreach (SYS_ROLEUSER roleUser in roleUserList)
                {
                    var roleName = SysRoleInfoRepository.FirstOrDefault(t => t.Id == roleUser.SYS_ROLEINFOID).CNAME;
                    if (!string.IsNullOrEmpty(roleName))
                        roleList.Add(roleName);
                }
                item.Roles = string.Join(",", roleList.ToArray());
            }
            return list;
        }
        public List<SysUserInfoPageResult> GetRoleUserList(SysUserInfoSearchModel search)
        {
            var userInfoList = SysUserInfoRepository.GetAllList();
            var list = new List<SysUserInfoPageResult>();
            
            userInfoList.ForEach(item => list.Add(new SysUserInfoPageResult
            {
                ID = item.Id,
                CName = item.CNAME,
                Memo = item.MEMO,
                State = item.STATE.ToString(),
                UserCode = item.USERCODE
            }));

            var roleUserList =   SysRoleUserRepository.GetAllList(i => i.SYS_ROLEINFOID == search.RoleId).Select(i => i.SYS_USERINFOID);

            list = list.Where(i => !roleUserList.Contains(i.ID)).ToList();

            return list;
        }
        public virtual PageList<SysUserInfoPageResult> GetRoleAuList(SysUserInfoSearchModel search)
        {
            return SysUserInfoRepository.GetRoleAuList(search);
        }

        public string validatorPassword(SysUserPwdModificationInfo sysUserInfo)
        {
            string oldpwd = MD5Encrypt32(sysUserInfo.OldPassword);
            var userinfo = SysUserInfoRepository.FirstOrDefault(t => t.Id == sysUserInfo.ID);
            if (oldpwd != userinfo.PASSWORD)
            {
                HelperBLL.ThrowEx("101", L["原始密码不正确！"]);
            }
            return "OK";
        }

        public virtual string Add(SysUserInfoData sysUserInfoEventData)
        {
            //判断是否存在相同的账号
            var sysUserInfo = SysUserInfoRepository.FirstOrDefault(t => t.USERCODE == sysUserInfoEventData.UserCode);
            if (sysUserInfo != null)
            {
                HelperBLL.ThrowEx("101", L["账号已存在，请重新输入一个新账号！"]);
            }

            SYS_USERINFO sysUserinfo = new SYS_USERINFO();          
            sysUserinfo.Id = Guid.NewGuid().ToString();
            sysUserinfo.USERCODE = sysUserInfoEventData.UserCode;
            sysUserinfo.CNAME = sysUserInfoEventData.CName;
            sysUserinfo.PASSWORD = MD5Encrypt32(sysUserInfoEventData.Password);
            sysUserinfo.STATE = 0;
            sysUserinfo.MEMO = sysUserInfoEventData.Memo;
            var returnId = SysUserInfoRepository.InsertAndGetId(sysUserinfo);
            if (string.IsNullOrEmpty(returnId))
            {
                return "保存失败!";
            }
            else
            {
                return "保存成功!";
            }
        }

        public PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return SysUserInfoRepository.GetComboxList(search);
        }

        public virtual string Update(SysUserInfoData sysUserInfoEventData)
        {
            //判断是否存在相同的账号
            var sysUserInfo = SysUserInfoRepository.FirstOrDefault(t => t.USERCODE == sysUserInfoEventData.UserCode && t.Id != sysUserInfoEventData.ID);
            if (sysUserInfo != null)
            {
                HelperBLL.ThrowEx("101", L["账号已存在，请重新输入一个新账号！"]);
            }
            int successCount = SysUserInfoRepository.Update(t => t.Id == sysUserInfoEventData.ID, t => new SYS_USERINFO()
            {
                USERCODE = sysUserInfoEventData.UserCode,
                CNAME = sysUserInfoEventData.CName,
               // PASSWORD = sysUserInfoEventData.Password,
               // STATE = sysUserInfoEventData.State,
                MEMO = sysUserInfoEventData.Memo,
            });
            if (successCount > 0)
            {
                return "更新成功";
            }
            else
            {
                return "更新失败";
            }
        }


        public virtual string PasswordModification(SysUserPwdModificationInfo sysUserInfoEventData)
        {
            //sysUserInfoEventData.ID = System.Web.HttpContext.Current.Session["uid"].ToString();
            if (string.IsNullOrEmpty(sysUserInfoEventData.ID)) {
                HelperBLL.ThrowEx("101", L["登录session失效了，请重新登录！"]);
            }
            #region 验证
            if (string.IsNullOrEmpty(sysUserInfoEventData.OldPassword)) {
                HelperBLL.ThrowEx("101", L["请输入原始密码！"]);
            }
            if (string.IsNullOrEmpty(sysUserInfoEventData.NewPassword))
            {
                HelperBLL.ThrowEx("101", L["请输入新密码！"]);
            }
            if (string.IsNullOrEmpty(sysUserInfoEventData.ConfirmPassword))
            {
                HelperBLL.ThrowEx("101", L["请输入确认密码！"]);
            }
            string oldpwd = MD5Encrypt32(sysUserInfoEventData.OldPassword);
            var userinfo = SysUserInfoRepository.FirstOrDefault(t => t.Id == sysUserInfoEventData.ID);
            if (oldpwd != userinfo.PASSWORD)
            {
                HelperBLL.ThrowEx("101", L["原始密码不正确！"]);
            }
            if (sysUserInfoEventData.NewPassword != sysUserInfoEventData.ConfirmPassword) {
                HelperBLL.ThrowEx("101", L["两次密码输入不一致！"]);
            }
            #endregion
            string newpwd= MD5Encrypt32(sysUserInfoEventData.NewPassword);
            int successCount = SysUserInfoRepository.Update(t => t.Id == sysUserInfoEventData.ID, t => new SYS_USERINFO()
            {
                
                PASSWORD = newpwd,
                               
            });
            if (successCount > 0)
            {
                return "密码更新成功";
            }
            else
            {
                return "密码更新失败";
            }
        }

        public string SaveRoleUser(SysRoleUserData sysRoleUserData)
        {
            var roleInfoIds = SysRoleUserRepository.GetAllList(t=>t.SYS_USERINFOID== sysRoleUserData.UserInfoID); //t => sysRoleUserData.Ids.Contains(t.SYS_ROLEINFOID)s
            foreach (var a in roleInfoIds)
            {
                SysRoleUserRepository.Delete(t => t.SYS_USERINFOID == a.SYS_USERINFOID);
            }

            if (sysRoleUserData != null) {
            List<SYS_ROLEUSER> roleUsers = new List<SYS_ROLEUSER>();

            foreach (var item in sysRoleUserData.Ids)
            {
                SYS_ROLEUSER roleUser = new SYS_ROLEUSER();
                roleUser.Id = Guid.NewGuid().ToString();
                roleUser.SYS_USERINFOID = sysRoleUserData.UserInfoID;
                roleUser.SYS_ROLEINFOID = item;
                roleUsers.Add(roleUser);
            }
         
            SysRoleUserRepository.Insert(roleUsers);
            }
            return "保存成功";
        }

        public virtual string Disable(SysUserInfoData sysUserInfoData)
        {
            var exsitData = SysUserInfoRepository.FirstOrDefault(t => t.Id == sysUserInfoData.ID);
            if (exsitData == null)
            {
                HelperBLL.ThrowEx("101", L["未查到对应数据"]);
            }
            if (exsitData.STATE != 0)
            {
                HelperBLL.ThrowEx("101", L["非启用状态的用户不能停用"]);
            }

            int successCount = SysUserInfoRepository.Update(t => t.Id == sysUserInfoData.ID, t => new SYS_USERINFO()
            {
                STATE = 2,
            });
            if (successCount > 0)
            {
                return "停用成功";
            }
            else
            {
                return "停用失败";
            }
        }

        public virtual string Enable(SysUserInfoData sysUserInfoData)
        {
            var exsitData = SysUserInfoRepository.FirstOrDefault(t => t.Id == sysUserInfoData.ID);
            if (exsitData == null)
            {
                HelperBLL.ThrowEx("101", L["未查到对应数据"]);
            }
            if (exsitData.STATE != 2)
            {
                HelperBLL.ThrowEx("101", L["非停用状态的用户不能启用"]);
            }

            int successCount = SysUserInfoRepository.Update(t => t.Id == sysUserInfoData.ID, t => new SYS_USERINFO()
            {
                STATE = 0,
            });
            if (successCount > 0)
            {
                return "启用成功";
            }
            else
            {
                return "启用失败";
            }
        }


        

       public virtual string PasswordReset(SysUserInfoData sysUserInfoData)
        {
            var exsitData = SysUserInfoRepository.FirstOrDefault(t => t.Id == sysUserInfoData.ID);
            if (exsitData == null)
            {
                HelperBLL.ThrowEx("101", L["请刷新界面再试下,数据可能已经被删除！！！"]);
            }
            string pwd = MD5Encrypt32("123456");  //后期初始密码配置数据字典
            int successCount = SysUserInfoRepository.Update(t => t.Id == sysUserInfoData.ID, t => new SYS_USERINFO()
            {
                PASSWORD = pwd
            });
            if (successCount > 0)
            {
                return "密码重置成功！";
            }
            else
            {
             
                return "密码重置失败!";
            }
        }

        public string Allot(AllotData allotData)
        {
            foreach (var item in allotData.IDs)
            {
                var sysRoleUser = SysRoleUserRepository.FirstOrDefault(t => t.SYS_USERINFOID == item);
                if (sysRoleUser == null)
                {
                    SYS_ROLEUSER sysRoleuser = new SYS_ROLEUSER();
                    sysRoleuser.SYS_USERINFOID = item;
                    sysRoleuser.SYS_ROLEINFOID = allotData.RoleID;
                    SysRoleUserRepository.InsertAndGetId(sysRoleuser);
                }
                else
                {
                    SysRoleUserRepository.Update(t => t.Id == sysRoleUser.Id,
                        t => new SYS_ROLEUSER() { SYS_ROLEINFOID = allotData.RoleID });
                }
            }
            return "成功";
        }

        public virtual string Login(SysLoginData sysLoginData)
        {
            var userInfo = SysUserInfoRepository.FirstOrDefault(t => t.USERCODE == sysLoginData.Account && t.PASSWORD == sysLoginData.Pwd);
            if (userInfo == null)
            {
                HelperBLL.ThrowEx("101", L["账号或密码不正确！"]);

            }
            #region 2019年11月15日  记录用户到seesion
           // System.Web.HttpContext.Current.Session["uid"] = userInfo.Id;
            #endregion
            return "登陆成功";

        }

        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        public virtual SysRoleUserInfo GetSysRoleByUser(SysUserInfoData sysUserInfoData)
        {
            if (sysUserInfoData == null || string.IsNullOrEmpty(sysUserInfoData.ID))
            {
                throw new BlocksBussnessException("101", L["未传入用户信息!"], null);
            }
            SysRoleUserInfo rtnData = new SysRoleUserInfo();
            var list = SysRoleUserRepository.GetAllList(x => x.SYS_USERINFOID == sysUserInfoData.ID).ToList();
            List<string> ids = new List<string>();
            foreach (var item in list)
            {
                ids.Add(item.SYS_ROLEINFOID);
            }
            rtnData.Ids = ids;
            return rtnData;

        }

        public virtual string DelAuList(SysRoleUserData sysRoleUserData)
        {
            for (var i = 0; i < sysRoleUserData.Ids.Count; i++)
            {
                var id = sysRoleUserData.Ids[i];
                var exsitData = SysRoleUserRepository.FirstOrDefault(t => t.SYS_USERINFOID == id);
                if (exsitData == null)
                {
                    throw new BlocksBussnessException("101", L["未查到对象"], null);
                }

                SysRoleUserRepository.Delete(t => t.SYS_USERINFOID == id);
              
            }
            return "删除成功！";
        }

        public string GetLoginUserId()
        {
            return UserContext.GetCurrentUser().UserId;
        }
    }
}
