public interface IGameTask {

	void Initialize(int goalStepsRequired);

	void TakeStep();

	void TakeGoalStep();

	void RevertGoalStep();
}
