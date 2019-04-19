/*   
 *   [optional multiple entries]
 *   Holder for the features which are inside a DmxModule.
 *   DmxModuleName – The name of the DmxModule defined in DmxModuleDefinitions
 */

namespace Carallon.MLibrary.Models.Dmx
{
    public class DmxModule
    {
        public string Name { get; set; }
        public int Instance { get; set; }
        public int TempElementModuleInstance { get; set; }
    }
}
