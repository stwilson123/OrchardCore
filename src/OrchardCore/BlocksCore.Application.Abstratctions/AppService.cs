namespace BlocksCore.Application.Abstratctions
{
    public abstract class AppService //: ApplicationService
    {

        public static string[] Postfixes = new string[] { "AppService", "ApplicationService" };


        //Move to Core 
        //        public TenantManager TenantManager { get; set; }
        //
        //        public UserManager UserManager { get; set; }
        //        
        //        protected virtual Task<User> GetCurrentUserAsync()
        //        {
        //            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
        //            if (user == null)
        //            {
        //                throw new ApplicationException("There is no current user!");
        //            }
        //
        //            return user;
        //        }
        //
        //        protected virtual Task<Tenant> GetCurrentTenantAsync()
        //        {
        //            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        //        }
        //
        //        protected virtual void CheckErrors(IdentityResult identityResult)
        //        {
        //            identityResult.CheckErrors(LocalizationManager);
        //        }
    }
}