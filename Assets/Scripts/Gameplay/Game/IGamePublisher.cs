public delegate void GameStateHandler(GameState newState);

public interface IGamePublisher {
	
	void Register(IGameListener gameListener);

	void Unregister(IGameListener gameListener);
}
