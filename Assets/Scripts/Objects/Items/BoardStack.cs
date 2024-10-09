namespace Scripts.Objects.Items
{
    public class BoardStack : CollectableItem
    {
        public override void Collect()
        {
            _audioManager.PlaySfx("boardTake");
            _matchManager.AddBoards(Value);

            Release();
        }
    }
}
