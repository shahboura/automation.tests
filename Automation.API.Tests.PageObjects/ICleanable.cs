using System.Threading.Tasks;

namespace Automation.API.Tests.PageObjects
{
    public interface ICleanable
    {
        /// <summary>
        /// Cleans resources created by page object.
        /// </summary>
        /// <returns></returns>
        Task Clean();
    }
}
