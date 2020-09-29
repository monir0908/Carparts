using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParts.Models.TempModels
{
    public class TempMasterProductSpecificationLabel
    {
        /// <summary>
        /// label Header Name
        /// </summary>
        public Guid? Id { get; set; }
        public string Model { get; set; }
        public string Label { get; set; }
        public string LabelName { get; set; }
        public bool IsHighlightedFeature { get; set; }
        public string IsHighlightedFeatureId { get; set; }
        public Guid? AdminId { get; set; }
        public DateTime AddedOn { get; set; }
        public bool IsCategorical { get; set; }
    }
}
