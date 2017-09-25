public delegate void CardClickHandler(CardBehaviour cardClicked);

public interface ICardClickPublisher {

	void Register(ICardClickListener clickListener);

	void Unregister(ICardClickListener clickListener);
}
