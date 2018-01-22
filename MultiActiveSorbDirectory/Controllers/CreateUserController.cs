using MultiActiveSorbDirectory.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MultiActiveSorbDirectory.Controllers
{
    public class CreateUserController : Controller
    {
        static DirectoryEntry createDirectoryEntry()
        {
            //Create directory connection with credentials
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");
            return directoryEntry;
        }

        private bool checkErrors(Account m)
        {
            if (m.c == null || m.department == null || m.employeeID == null ||
                m.givenName == null || m.initials == null || m.l == null || 
                m.mail == null || m.manager == null || m.mobile == null || 
                m.postalCode == null || m.sAMAccountName == null || m.SN == null || 
                m.st == null || m.streetAddress == null || m.telephoneNumber == null || 
                m.title == null ||  m.physicalDeliveryOfficeName == null)
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

        private string resetPasswordFromName(String sAMAccountName)
        {
            try
            {
                DirectoryEntry myLdapConnection = createDirectoryEntry();
                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(sAMAccountName=" + sAMAccountName + ")";
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    DirectoryEntry entryToUpdate = result.GetDirectoryEntry();
                    entryToUpdate.Invoke("SetPassword", new object[] { "Mti@325" });
                    entryToUpdate.Properties["LockOutTime"].Value = 0;
                    entryToUpdate.Properties["pwdLastSet"].Value = 0;
                    entryToUpdate.CommitChanges();
                    return "";
                }

                else return "error";
            }

            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private string enableUserFromName(String sAMAccountName)
        {
            try
            {
                DirectoryEntry myLdapConnection = new DirectoryEntry("LDAP://OU=Users,OU=Harlem Road,DC=multisorb,DC=com", "Administrator", "325H@l3m!");

                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(sAMAccountName=" + sAMAccountName + ")";
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    DirectoryEntry entryToUpdate = result.GetDirectoryEntry();
                    entryToUpdate.Invoke("Put", new object[] { "userAccountControl", "512" });
                    entryToUpdate.CommitChanges();
                    return "";
                }

                else return "error";
            }

            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private string createUserNow(Account m)
        {
            // connect to LDAP  

            DirectoryEntry myLdapConnection = createDirectoryEntry();

            // define vars for user  

            DirectoryEntry user = myLdapConnection.Children.Add(
                                 "CN=" + m.SN + "\\, " + m.givenName + " " +m.initials+ ".", "user");

            // User name (domain based)   
            user.Properties["userprincipalname"].Add(m.sAMAccountName + "@multisorb.com");

            // User name (older systems)  
            user.Properties["samaccountname"].Add(m.sAMAccountName);

            // Surname  
            user.Properties["sn"].Add(m.SN);

            // Initials
            user.Properties["initials"].Add(m.initials);

            // Forename  
            user.Properties["givenname"].Add(m.givenName);

            // Display name
            user.Properties["displayname"].Add(m.SN + ", " + m.givenName + " " + m.initials + ".");

            // E-mail  
            user.Properties["mail"].Add(m.mail + "@multisorb.com");

            // E-mail  
            user.Properties["mailNickName"].Add(m.mail);

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
            user.Properties["physicalDeliveryOfficeName"].Add("Ext. "+m.physicalDeliveryOfficeName);

            //Mobile Phone
            user.Properties["mobile"].Add(m.mobile);

            //Company
            user.Properties["company"].Add("Multisorb");

            //Logon Script //might be breaking
            user.Properties["scriptPath"].Add("login.vbs");

            //EmployeeID //might be breaking
            user.Properties["employeeID"].Add(m.employeeID);

            //Unlock account
            user.Properties["LockOutTime"].Value = 0;

            //Make them change password
            user.Properties["pwdLastSet"].Value = 0;

            //Manager
            String managerDN = getDistinguishedName(m.manager, myLdapConnection);
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
            catch(Exception e)
            {
                return e.ToString();
            }
            return "";
        }

        private string createTicket(String displayName)
        {
            MailMessage msg = new MailMessage();
            //msg.To.Add(new MailAddress("nsopko@multisorb.com"));
            msg.To.Add(new MailAddress("help@multisorb.on.spiceworks.com"));
            msg.From = new MailAddress("no-reply@multisorb.com");
            msg.Subject = "New User Account Created";
            //msg.Body = "Please finish setup for: " + displayName;
            msg.Body = "A new employee user account has been created in Active Directory!\n Now please complete setup for: ";
            //msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential("nsopko@multisorb.com", "MTISummer2017!");
            client.Port = 25; // You can use Port 25 if 587 is blocked (mine is!)
            //client.Host = "smtp.office365.com";
            client.Host = "multisorb-com.mail.protection.outlook.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            ServicePointManager
    .ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;
            try
            {
                client.Send(msg);
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // GET: CreateUser
        public ActionResult Index()
        {
            return View();     
        }

        //POST: Check CN Usage AJAX
        [HttpPost]
        public JsonResult checkCN(String cn)
        {
            if (cn == "")
            {
                return Json(new { success = false, error = "CN cannot be blank" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DirectoryEntry myLdapConnection = createDirectoryEntry();

                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(CN=" + cn + ")";
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
                else return Json(new { success = false, error="Alias taken already" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(new { success = false, error = "Show this to an IT person please: "+e.ToString() }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Index(Account obj)
        {
            if (checkErrors(obj))
            {
                return View();
            }
            else
            {
                var returner = createUserNow(obj);
                if (returner == "")
                {
                    returner = resetPasswordFromName(obj.sAMAccountName);
                    if (returner == "")
                    {
                        returner = enableUserFromName(obj.sAMAccountName);
                        if (returner == "")
                        {
                            returner = createTicket(obj.displayName); 
                            if (returner =="")
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.error = returner;
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.error = returner;
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.error = returner;
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = returner;
                    return View();
                }
            }
        }

        //POST: Get Manager Drop Down
        [HttpPost]
        public string populateSupervisors()
        {
            List<Person> people = new List<Person>();

            DirectoryEntry de = createDirectoryEntry();

            ActiveDirectoryAccount viewModel = new ActiveDirectoryAccount();
            List<Account> allUsersView = new List<Account>();

            DirectorySearcher userSearcher = new DirectorySearcher(de);
            userSearcher.SearchScope = SearchScope.OneLevel;
            userSearcher.Filter = "(&(objectCategory=person)(objectClass=user))";
            foreach (SearchResult user in userSearcher.FindAll())
            {
                Person inquestion = new Person();
                inquestion.displayName = user.Properties["displayName"][0].ToString();
                inquestion.sAMAccountName = user.Properties["sAMAccountName"][0].ToString();
                people.Add(inquestion);
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(people);
        }
    }
}