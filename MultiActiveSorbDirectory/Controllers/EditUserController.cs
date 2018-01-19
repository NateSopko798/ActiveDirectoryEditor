using MultiActiveSorbDirectory.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MultiActiveSorbDirectory.Controllers
{
    public class EditUserController : Controller
    {
        static DirectoryEntry createDirectoryEntry()
        {
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");
            return directoryEntry;
        }

        private Account buildUser(SearchResult user)
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
                m.employeeID = user.Properties["employeeID"][0].ToString();
            }
            catch
            {
                m.employeeID = "";
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
                m.mailNickname = user.Properties["mailNickname"][0].ToString();
            }
            catch
            {
                m.mailNickname = "";
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
                m.physicalDeliveryOfficeName = user.Properties["physicalDeliveryOfficeName"][0].ToString();
            }
            catch
            {
                m.physicalDeliveryOfficeName = "";
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
            try
            {
                m.initials = user.Properties["initials"][0].ToString();
            }
            catch
            {
                m.initials = "";
            }
            if (m.manager != "")
            {
                string managerAlias = getAlias(m.manager);
                m.managerAlias = managerAlias;
            }
            return m;
        }

        private string getDistinguishedName(String user)
        {
            DirectorySearcher searcher = new DirectorySearcher(createDirectoryEntry())
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user + "))"
            };

            var result = searcher.FindOne();

            if (result == null)
            {
                return "";
            }
            try
            {
                string DN = "";

                if (result.Properties.Contains("distinguishedName"))
                {
                    DN = result.Properties["distinguishedName"][0].ToString();
                }
                return DN;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        //private string getDisplayName(String user)
        //{
        //    DirectorySearcher searcher = new DirectorySearcher(createDirectoryEntry())
        //    {
        //        PageSize = int.MaxValue,
        //        Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user + "))"
        //    };

        //    var result = searcher.FindOne();

        //    if (result == null)
        //    {
        //        return "";
        //    }
        //    try
        //    {
        //        string displayName = "";

        //        if (result.Properties.Contains("distinguishedName"))
        //        {
        //            DN = result.Properties["distinguishedName"][0].ToString();
        //        }
        //        return DN;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //        return "";
        //    }
        //}

        private Account searchByUserName(String user, DirectoryEntry de)
        {
            DirectorySearcher searcher = new DirectorySearcher(de)
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user + "))"
            };

            var result = searcher.FindOne();

            if (result == null)
            {
                return null;
            }
            return buildUser(result);
        }

        private string getAlias(String manager)
        {
            DirectorySearcher searcher = new DirectorySearcher(createDirectoryEntry())
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(distinguishedName=" + manager + "))"
            };

            var result = searcher.FindOne();

            if (result == null)
            {
                return null;
            }
            return result.Properties["displayName"][0].ToString();
        }

        private bool checkErrors(Account m)
        {
            if (m.c == null || m.department == null || m.employeeID == null ||
                m.givenName == null || m.initials == null || m.l == null ||
                m.mail == null || m.manager == null || m.mobile == null ||
                m.postalCode == null || m.sAMAccountName == null || m.SN == null ||
                m.st == null || m.streetAddress == null || m.telephoneNumber == null ||
                m.title == null || m.physicalDeliveryOfficeName == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string getDistinguishedName(String user, DirectoryEntry de)
        {
            DirectorySearcher searcher = new DirectorySearcher(de)
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user + "))"
            };

            var result = searcher.FindOne();

            if (result == null)
            {
                return "";
            }
            try
            {
                string DN = "";

                if (result.Properties.Contains("distinguishedName"))
                {
                    DN = result.Properties["distinguishedName"][0].ToString();
                }
                return DN;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        private string editUserNow(Account m)
        {
            DirectoryEntry de = createDirectoryEntry();

            DirectorySearcher searcher = new DirectorySearcher(de)
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + m.sAMAccountName + "))"
            };
            
            DirectoryEntry user = searcher.FindOne().GetDirectoryEntry();

            string newCN = "CN=" + m.SN + "\\, " + m.givenName + " " + m.initials + ".";
            try
            {
                using (user)
                {
                    user.Rename(newCN);
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            // Surname  
            user.Properties["sn"].Add(m.SN);

            // Initials
            user.Properties["initials"].Add(m.initials);

            // Forename  
            user.Properties["givenname"].Add(m.givenName);

            // Display name
            user.Properties["displayname"].Add(m.SN + ", " + m.givenName + " " + m.initials + ".");

            //Country
            user.Properties["c"].Add(m.c);

            //title
            user.Properties["title"].Add(m.title);

            //Department
            user.Properties["department"].Add(m.department);

            //Telephone number
            user.Properties["telephoneNumber"].Add(m.telephoneNumber);

            //Street
            user.Properties["streetAddress"].Add(m.streetAddress);

            //City
            user.Properties["l"].Add(m.l);

            //State
            user.Properties["st"].Add(m.st);

            //Postal Code
            user.Properties["postalCode"].Add(m.postalCode);

            //Office
            user.Properties["physicalDeliveryOfficeName"].Add("Ext. " + m.physicalDeliveryOfficeName);

            //Mobile Phone
            user.Properties["mobile"].Add(m.mobile);

            //EmployeeID //might be breaking
            user.Properties["employeeID"].Add(m.employeeID);

            //Manager
            String managerDN = getDistinguishedName(m.manager, de);
            if (managerDN == "")
            {
                return "Manager not found";
            }
            else
            {
                user.Properties["manager"].Add(managerDN);
            }

            //Apply changes to new user
            try
            {
                user.CommitChanges();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "";
        }

        // GET: EditUser
        public ActionResult Index()
        {
            return View();
        }
        
        //POST: Fill Form
        [HttpPost]
        public String fillForm(string alias)
        {
            Account m = new Account();

            DirectoryEntry de = createDirectoryEntry();

            m = searchByUserName(alias, de);

            JavaScriptSerializer ser = new JavaScriptSerializer();

            return ser.Serialize(m);
        }

        //POST: Fill Manager
        [HttpPost]
        public String fillManager(string manager)
        {
            Account m = new Account();
            
            m.displayName = manager;

            JavaScriptSerializer ser = new JavaScriptSerializer();

            return ser.Serialize(m);
        }

        //POST: Check Alias Usage AJAX
        [HttpPost]
        public JsonResult checkAlias(String alias)
        {
            if (alias == "")
            {
                return Json(new { success = false, error = "Alias cannot be blank" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DirectoryEntry myLdapConnection = createDirectoryEntry();

                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(sAMAccountName=" + alias + ")";
                SearchResult result = search.FindOne();

                if (result == null)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { success = false, error = "Alias taken already" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(new { success = false, error = "Show this to an IT person please: " + e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        //POST: Check Email Usage AJAX
        [HttpPost]
        public JsonResult checkEmail(String mail)
        {
            try
            {
                DirectoryEntry myLdapConnection = createDirectoryEntry();

                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(mail=" + mail + ")";
                SearchResult result = search.FindOne();

                if (result == null)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { success = false, error = "Email address taken already" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(new { success = false, error = "Show this to an IT person please: " + e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        //POST: Form Post Back
        [HttpPost]
        public ActionResult editUser(Account obj)
        {
            if (checkErrors(obj))
            {
                ViewBag.error = "One of the account values were missing";
                return View();
            }
            else
            {
                var returner = editUserNow(obj);
                if (returner == "")
                {
                    ViewBag.error = returner;
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
        }
    }
}