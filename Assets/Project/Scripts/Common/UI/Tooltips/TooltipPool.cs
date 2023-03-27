namespace Common.UI.Tooltips
{
    public class TooltipPool : Pool<Tooltip>
    {
        public Tooltip ShowToolTip(UnityEngine.Vector3 position, string info)
        {
            var tooltipObject = Get();

            tooltipObject.transform.position = position;
            tooltipObject.Show(info);

            return tooltipObject;
        }
        
        protected override void OnGetPool(Tooltip element)
        {
            element.gameObject.SetActive(true);
            element.transform.SetParent(Origin);
        }

        protected override void OnReleasePool(Tooltip element)
        {
            element.gameObject.SetActive(false);
            element.transform.SetParent(Origin);
        }

        protected override void OnDestroyPool(Tooltip element)
        {
            Destroy(element.gameObject);
        }
    }
}
