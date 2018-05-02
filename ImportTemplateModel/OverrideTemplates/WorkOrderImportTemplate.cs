using MEXModel;

namespace ImportTemplateModel
{
    internal class WorkOrderImportTemplate : ImportTemplate<WorkOrder>
    {
        public WorkOrderImportTemplate()
        {
            RequiredPropertyNames = new string[] {
                nameof(entity.AssetID),
                nameof(entity.WorkOrderFormatName),
                nameof(entity.WorkOrderDescription),
                nameof(entity.WorkOrderNumber)
            };
        }

        public override object CreateTemplateObject()
        {
            WorkOrder wo = new WorkOrder();

            wo.WorkOrderStatusID = 1;
            wo.WorkOrderFormatName = "Standard";

            return wo;
        }
    }
}
