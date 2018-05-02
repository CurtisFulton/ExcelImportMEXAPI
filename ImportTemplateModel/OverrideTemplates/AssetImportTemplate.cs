using MEXModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportTemplateModel
{
    internal class AssetImportTemplate : ImportTemplate<Asset>
    {
        public AssetImportTemplate()
        {
            RequiredPropertyNames = new string[] {
                nameof(entity.AssetNumber),
                nameof(entity.AssetDescription),
            };
            
            IgnoredPropertyNames = new string[] {
                nameof(entity.CatalogueID),
                nameof(entity.StoreID),
                nameof(entity.BinLocationID),
                nameof(entity.AreaOfPlant),
                nameof(entity.MaximumQuantity),
                nameof(entity.MinimumQuantity),
                nameof(entity.UnitPrice),
                nameof(entity.MarkupTypeName),
                nameof(entity.MarkupAmount),
                nameof(entity.TaxID),
                nameof(entity.ShelfLifeDays),
                nameof(entity.IsSerialisedCatalogueItem),
                nameof(entity.IsSerialisedCatalogueItem),
                nameof(entity.WizardSizeHeight),
                nameof(entity.WizardSizeWidth),
                nameof(entity.WizardLocationX),
                nameof(entity.WizardLocationY),
                nameof(entity.IsAssetAddedtoWizard),
                nameof(entity.PurchaseCost),
                nameof(entity.SequenceNumber),
                nameof(entity.IsAsset),
            };
        }

        public override object CreateTemplateObject()
        {
            Asset asset = new Asset();

            asset.IsActive = true;
            asset.IsAsset = true;

            return asset;
        }
    }
}
