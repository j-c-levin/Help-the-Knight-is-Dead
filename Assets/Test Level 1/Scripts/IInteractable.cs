namespace HKD_1
{
	public enum TapState
	{
		NONE,
		BUTTON_DOWN,
		BUTTON_PRESSED,
		BUTTON_UP
	}

	public interface IInteractable
	{
		void Interact (PlayerController player);

		void Damage (int damage);

		bool IsBlocking ();
	}
}