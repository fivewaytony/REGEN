using System;

public class AlertManager
{
    public string Title
    {
        get;
        private set;
    }

    public string Message
    {
        get;
        private set;
    }

    public Action Callback
    {
        get;
        private set;
    }

    public AlertManager(string title, string message, Action callback = null) //생성자
	{
        this.Title = title;
        this.Message = message;
        this.Callback = callback;
    }

}
