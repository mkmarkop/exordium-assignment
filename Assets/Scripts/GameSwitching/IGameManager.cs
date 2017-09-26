public interface IGameManager {

	void ToggleTimer();

	void LoadGame(AbstractGame game);

	void ExitGame();

	void Register(IGameManagerListener managerListener);

	void Unregister(IGameManagerListener managerListener);
}
