public delegate void GameStateHandler(GameState newState);

public interface IGamePublisher {

	event GameStateHandler OnGameStateChange;

	void Register(IGameListener gameListener);

	void Unregister(IGameListener gameListener);
}
