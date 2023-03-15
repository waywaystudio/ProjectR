using Common;
using Common.Skills;

namespace Adventurers.Rogue.Skills
{
    public class Liberation : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();
        
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        protected void OnEnable()
        {
            // armorCrash.Initialize(Provider);

            OnActivated.Register("StartCooling", StartCooling);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);
        }
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            // MainGame.MainData.StatusEffectMaster.Get((DataIndex)skillData.StatusEffect, out var armorCrashObject);
            //
            // if (!TryGetComponent(out armorCrash)) armorCrash = gameObject.AddComponent<StatusEffectCompletion>();
            //
            // armorCrash.SetProperties(armorCrashObject, 16);
        }
#endif
    }
}
