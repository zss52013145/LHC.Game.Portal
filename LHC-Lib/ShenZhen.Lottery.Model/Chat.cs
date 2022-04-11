using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShenZhen.Lottery.Model
{
    public class Chat
    {
        public int Id { get; set; }

        public int lType { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public DateTime SubTime { get; set; }



        public string ShowUserName {

            get {

                //zss588



                int len = UserName.Length;

                if (len == 0) return "";


                //6
                int liangbian = (len - 2) / 2;


                string result = UserName.Substring(0, liangbian) + "**" + UserName.Substring(liangbian + 2);


                return result;

            }
        }


    }
}
