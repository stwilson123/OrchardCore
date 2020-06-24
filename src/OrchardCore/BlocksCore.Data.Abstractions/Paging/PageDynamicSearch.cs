using System.Collections.Generic;
using BlocksCore.Abstractions.Data.Paging;

namespace BlocksCore.Data.Abstractions.Paging
{
    public class PageDynamicSearch
    {
        public  static string getStringForGroup(IGroup group,List<DbParam> listDbParam)
        {
         //   var alias = group.rules.Select(t =>  t.field.Contains('.') ?  t.field.Substring(0,t.field.IndexOf('.')) : "").Where(t => !string.IsNullOrEmpty(t));
            var s = "(";
            if (group.groups != null) {
                for (var index = 0; index < group.groups.Count; index++) {
                    if (s.Length > 1) {
                        s += " " + group.groupOp + " ";
                    }
                    try {
                        s += getStringForGroup(group.groups[index],null);
                    } catch{throw;}
                }
            }

            if (group.rules != null) {
                try{
                    for (var index = 0; index < group.rules.Count; index++) {
                        if (s.Length > 1) {
                            s += " " + group.groupOp + " ";
                        }
                        s += getStringForRule(group.rules[index]);
                    }
                } catch { throw;}
            }

            s += ")";

            if (s == "()") {
                return ""; // ignore groups that don't have rules
            }
         
            return s;
        }
        
        public static string getStringForRule (IRule rule)
        {
            var opUF = "";
            var opC = "";
            var cm = "";
            var ret = "";var val = "";

            if (IRule.opend.ContainsKey(rule.op))
            {
                opUF = IRule.opend[rule.op];
                opC = rule.op;
            }
            cm = rule.field;
            val = rule.data;
            //if (opC == "bw" || opC == "bn") { val = val + "%"; }
            //if (opC == "ew" || opC == "en") { val = "%" + val; }
            //if (opC == "cn" || opC == "nc") { val = "%" + val + "%"; }
            //if (opC == "in" || opC == "ni") { val = " (" + val + ")"; }

            //            if(p.errorcheck) { checkData(rule.data, cm); }
            //            if($.inArray(cm.searchtype, numtypes) != -1 || opC == 'nn' || opC == 'nu') { ret = rule.field + " " + opUF + " " + val; }
             ret = string.Format(opUF,rule.field,"(\"" + val + "\")");
            return ret;
        }
    }
    
    public class DbParam
    {
        public string param { set; get; }
        
        public object value { set; get; }

    }
}