using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using MultiActiveSorbDirectory.Models;

namespace MultiActiveSorbDirectory.Controllers
{
    public class HomeController : Controller
    {
        //List<String> propList = new List<string>();
        //List<String> valList = new List<string>();
        //List<String> userList = new List<string>();

        static DirectoryEntry createDirectoryEntry()
        {
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");
            return directoryEntry;
        }

        public ActionResult searchByUserName(String user, DirectoryEntry de)
        {
            DirectorySearcher searcher = new DirectorySearcher(de)
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName="+user+"))"
            };

            var result = searcher.FindOne();

            if (result == null)
            {
                Console.Write("result was null");
            }

            try
            {
                foreach (string propName in result.Properties.PropertyNames)
                {
                    ResultPropertyValueCollection valueCollection =
                    result.Properties[propName];
                    foreach (Object propertyValue in valueCollection)
                    {

                        //propList.Add(propName);
                        //valList.Add(propertyValue.ToString());
                    }
                }

                string surname;

                if (result.Properties.Contains("sn"))
                {
                    surname = result.Properties["sn"][0].ToString();
                    ViewBag.lastname = surname;
                }

                string firstname;

                if (result.Properties.Contains("givenName"))
                {
                    firstname = result.Properties["givenName"][0].ToString();
                    ViewBag.firstname = firstname;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception caught:\n\n" + e.ToString());
            }

            //ViewBag.propList = propList;
            //ViewBag.valList = valList;
            return View();
        }

        public Account buildUser(SearchResult user)
        {
            Account m = new Account();
            try
            {
                m.distinguishingName = user.Properties["DN"][0].ToString();
            }
            catch
            {
                m.distinguishingName = "";
            }
            try
            {
                m.c = user.Properties["C"][0].ToString();
            }
            catch
            {
                m.c = "";
            }
            try
            {
                m.CN = user.Properties["CN"][0].ToString();
            }
            catch
            {
                m.CN = "";
            }
            try
            {
                m.company = user.Properties["company"][0].ToString();
            }
            catch
            {
                m.company = "";
            }
            try
            {
                m.department = user.Properties["department"][0].ToString();
            }
            catch
            {
                m.department = "";
            }
            try
            {
                m.displayName = user.Properties["displayName"][0].ToString();
            }
            catch
            {
                m.displayName = "";
            }

            try
            {
                m.givenName = user.Properties["givenName"][0].ToString();
            }
            catch
            {
                m.givenName = "";
            }
            try
            {
                m.homephone = user.Properties["homephone"][0].ToString();
            }
            catch
            {
                m.homephone = "";
            }
            try
            {
                m.l = user.Properties["l"][0].ToString();
            }
            catch
            {
                m.l = "";
            }
            try
            {
                m.mail = user.Properties["mail"][0].ToString();
            }
            catch
            {
                m.mail = "";
            }
            try
            {
                m.mailNickname = user.Properties["mailNickname"][0].ToString();
            }
            catch
            {
                m.mailNickname = "";
            }
            try
            {
                m.manager = user.Properties["manager"][0].ToString();
            }
            catch
            {
                m.manager = "";
            }
            try
            {
                m.mobile = user.Properties["mobile"][0].ToString();
            }
            catch
            {
                m.mobile = "";
            }
            try
            {
                m.msExchHomeServerName = user.Properties["msExchHomeServerName"][0].ToString();
            }
            catch
            {
                m.msExchHomeServerName = "";
            }
            try
            {
                m.name = user.Properties["name"][0].ToString();
            }
            catch
            {
                m.name = "";
            }
            try
            {
                m.objectCategory = user.Properties["objectCategory"][0].ToString();
            }
            catch
            {
                m.objectCategory = "";
            }
            try
            {
                m.ObjectClass = user.Properties["ObjectClass"][0].ToString();
            }
            catch
            {
                m.ObjectClass = "";
            }
            try
            {
                m.postalCode = user.Properties["postalCode"][0].ToString();
            }
            catch
            {
                m.postalCode = "";
            }
            try
            {
                m.pwdLastSet = user.Properties["pwdLastSet"][0].ToString();
            }
            catch
            {
                m.pwdLastSet = "";
            }
            try
            {
                m.sAMAccountName = user.Properties["sAMAccountName"][0].ToString();
            }
            catch
            {
                m.sAMAccountName = "";
            }
            try
            {
                m.showInAddressBook = user.Properties["showInAddressBook"][0].ToString();
            }
            catch
            {
                m.showInAddressBook = "";
            }
            try
            {
                m.SN = user.Properties["SN"][0].ToString();
            }
            catch
            {
                m.SN = "";
            }
            try
            {
                m.st = user.Properties["st"][0].ToString();
            }
            catch
            {
                m.st = "";
            }
            try
            {
                m.streetAddress = user.Properties["streetAddress"][0].ToString();
            }
            catch
            {
                m.streetAddress = "";
            }
            try
            {
                m.telephoneNumber = user.Properties["telephoneNumber"][0].ToString();
            }
            catch
            {
                m.telephoneNumber = "";
            }
            try
            {
                m.title = user.Properties["title"][0].ToString();
            }
            catch
            {
                m.title = "";
            }
            try
            {
                m.userAccountControl = user.Properties["userAccountControl"][0].ToString();
            }
            catch
            {
                m.userAccountControl = "";
            }
            try
            {
                m.userPrincipalName = user.Properties["userPrincipalName"][0].ToString();
            }
            catch
            {
                m.userPrincipalName = "";
            } 
            return m;
        }

        public Account buildUserBetter(SearchResult user)
        {
            Account m = new Account();
            m.c = user.Properties["C"][0].ToString();
            m.CN = user.Properties["CN"][0].ToString();
            m.company = user.Properties["company"][0].ToString();
            m.department = user.Properties["department"][0].ToString();
            m.displayName = user.Properties["displayName"][0].ToString();
            m.givenName = user.Properties["givenName"][0].ToString();
            m.homephone = user.Properties["homephone"][0].ToString();
            m.l = user.Properties["l"][0].ToString();
            m.mail = user.Properties["mail"][0].ToString();
            m.mailNickname = user.Properties["mailNickname"][0].ToString();
            m.manager = user.Properties["manager"][0].ToString();
            m.mobile = user.Properties["mobile"][0].ToString();
            m.msExchHomeServerName = user.Properties["msExchHomeServerName"][0].ToString();
            m.name = user.Properties["name"][0].ToString();
            m.objectCategory = user.Properties["objectCategory"][0].ToString();
            m.ObjectClass = user.Properties["ObjectClass"][0].ToString();
            m.postalCode = user.Properties["postalCode"][0].ToString();
            m.pwdLastSet = user.Properties["pwdLastSet"][0].ToString();
            m.sAMAccountName = user.Properties["sAMAccountName"][0].ToString();
            m.showInAddressBook = user.Properties["showInAddressBook"][0].ToString();
            m.SN = user.Properties["SN"][0].ToString();
            m.st = user.Properties["st"][0].ToString();
            m.streetAddress = user.Properties["streetAddress"][0].ToString();
            m.telephoneNumber = user.Properties["telephoneNumber"][0].ToString();
            m.title = user.Properties["title"][0].ToString();
            m.userAccountControl = user.Properties["userAccountControl"][0].ToString();
            m.userPrincipalName = user.Properties["userPrincipalName"][0].ToString();
            return m;
        }

        public ActionResult searchAllUsers(DirectoryEntry de)
        {
            ActiveDirectoryAccount viewModel = new ActiveDirectoryAccount();
            List<Account> allUsersView = new List<Account>();

            DirectorySearcher userSearcher = new DirectorySearcher(de);
            userSearcher.SearchScope = SearchScope.OneLevel; // don't recurse down
            userSearcher.Filter = "(&(objectCategory=person)(objectClass=user))";

            foreach (SearchResult user in userSearcher.FindAll())
            {
                //userList.Add(user.Properties["givenName"][0].ToString());
                allUsersView.Add(buildUser(user));
            }
            //ViewBag.userList = userList;
            viewModel.Accounts = allUsersView;
            return View(viewModel);
        }

        public ActionResult Index()
        {
            return searchAllUsers(createDirectoryEntry());
            //return searchByUserName("nsopko",createDirectoryEntry());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}