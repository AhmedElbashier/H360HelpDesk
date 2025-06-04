using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

public class LicenseManager
{
    public int GetAdminUserLimit(string licenseFilePath)
    {
        try
        {
            if (File.Exists(licenseFilePath))
            {
                string json = File.ReadAllText(licenseFilePath);
                var license = JsonConvert.DeserializeObject<License>(json);
                return license.AdminsLimit;
            }
            else
            {
                // Handle the case where the license file is missing
                return -1; // or any default value
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during file reading or deserialization
            return (-2); // or any default value
        }
    }
    public int GetAgentUserLimit(string licenseFilePath)
    {
        try
        {
            if (File.Exists(licenseFilePath))
            {
                string json = File.ReadAllText(licenseFilePath);
                var license = JsonConvert.DeserializeObject<License>(json);
                return license.AgentsLimit;
            }
            else
            {
                // Handle the case where the license file is missing
                return -1; // or any default value
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during file reading or deserialization
            return (-2); // or any default value
        }
    }

    public int GetSupervisorUserLimit(string licenseFilePath)
    {
        try
        {
            if (File.Exists(licenseFilePath))
            {
                string json = File.ReadAllText(licenseFilePath);
                var license = JsonConvert.DeserializeObject<License>(json);
                return license.SupervisorsLimit;
            }
            else
            {
                // Handle the case where the license file is missing
                return -1; // or any default value
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during file reading or deserialization
            return (-2); // or any default value
        }
    }

        public int GetBackOfficeUserLimit(string licenseFilePath)
        {
            try
            {
                if (File.Exists(licenseFilePath))
                {
                    string json = File.ReadAllText(licenseFilePath);
                    var license = JsonConvert.DeserializeObject<License>(json);
                    return license.BackOfficeLimit;
                }
                else
                {
                    // Handle the case where the license file is missing
                    return -1; // or any default value
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file reading or deserialization
                return (-2); // or any default value
            }
        }
    public int GetAllUsersLimit(string licenseFilePath)
    {
        try
        {
            if (File.Exists(licenseFilePath))
            {
                string json = File.ReadAllText(licenseFilePath);
                var license = JsonConvert.DeserializeObject<License>(json);
                return license.BackOfficeLimit+license.SupervisorsLimit+ license.AgentsLimit+ license.AdminsLimit;
            }
            else
            {
                // Handle the case where the license file is missing
                return -1; // or any default value
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during file reading or deserialization
            return (-2); // or any default value
        }
    }
}
