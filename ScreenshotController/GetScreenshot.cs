using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;


namespace AllClassifieds.ScreenshotController
{
    public class GetScreenshot
    {
           public static string Capture(IWebDriver driver, string screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string finalpth = pth.Substring(0, pth.LastIndexOf("bin")) + "Screenshots\\" + screenShotName + ".png";
            string localpath = new Uri(finalpth).LocalPath;
            screenshot.SaveAsFile(localpath, ScreenshotImageFormat.Png);
            return localpath;
        }
    }
}
