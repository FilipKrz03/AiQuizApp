using Blazored.Modal;

namespace Client.Common
{
	public static class ModalOptionsHelper
	{
		public static ModalOptions GetForResultPopup()
		{
			return new ModalOptions()
			{
				HideHeader = true,
				Position = ModalPosition.BottomRight,
				AnimationType = ModalAnimationType.PopIn
			};
		}
	}
}
