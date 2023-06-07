namespace Common.Characters
{
    public class VillainData : CharacterData
    {
        // private DifficultyLevel difficulty;
        
        public void SetDifficulty(int difficulty)
        {
            // Create DifficultyType? or Level?
            // CharacterCombatStatus를 Venturer 와 Villain으로 나눈 후,
            // Villain.CombatStatus.Initialize에서 Difficulty를 통한 초기 값 설정.
            // Villain의 Difficulty는 단순한 StatSpec뿐만 아니라 드랍 아이템, 사용 스킬 등이 달리질 수 있다. 
        }
    }
}
