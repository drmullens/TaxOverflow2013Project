using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxOverflow2013.Models
{
    public class CurrentData
    {
        string mCurrentUser;
        int mCurrentUserID;
        int mCurrentQueID;

        public int CurrentUserID 
        {
            get { return mCurrentUserID; }
            set { mCurrentUserID = value;}
        }

        public string CurrentUser
        {
            get { return mCurrentUser; }
            set { mCurrentUser = value; }
        }

        public int CurrentQueID
        {
            get { return mCurrentQueID; }
            set { mCurrentQueID = value; }
        }
    }
}