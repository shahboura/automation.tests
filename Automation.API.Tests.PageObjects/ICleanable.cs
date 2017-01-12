using System.Threading.Tasks;

namespace Automation.API.Tests.Resources
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
