using Core;

namespace Character.Actions.Assassin
{
    public class Liberation : SkillComponent
    {
        public override ICombatTaker MainTarget => CharacterSystem.Searching.GetSelf();
        
        public override void Release() { }
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
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
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            // MainGame.MainData.StatusEffectMaster.Get((DataIndex)skillData.StatusEffect, out var armorCrashObject);
            //
            // if (!TryGetComponent(out armorCrash)) armorCrash = gameObject.AddComponent<StatusEffectCompletion>();
            //
            // armorCrash.SetProperties(armorCrashObject, 16);
        }
#endif
    }
}
