using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.Logs;

public static class LogsDictionary
{
    public static Dictionary<LogsEnum, string> Get = new()
    {
        { LogsEnum.Ban, "Сотрудник {0} забанил пользователя {1}" },
        { LogsEnum.Unban, "Сотрудник {0} разбанил пользователя {1}" },
        { LogsEnum.ShadowBan, "Сотрудник {0} выдал теневой бан пользователю {1}" },
        { LogsEnum.UnShadowBan, "Сотрудник {0} отменил теневой бан пользователю {1}" },
        
        { LogsEnum.Logging, "Сотрудник {0} зашел в аккаунт" },
        
        { LogsEnum.CreateNews, "Сотрудник {0} создал новость {1}" },
        { LogsEnum.EditNews, "Сотрудник {0} отредактировал новость {1}" },
        { LogsEnum.DeleteNews, "Сотрудник {0} удалил новость {1}" },
        
        { LogsEnum.CreateEvent, "Сотрудник {0} создал ивент {1}" },
        { LogsEnum.EditEvent, "Сотрудник {0} изменил ивент {1}" },
        { LogsEnum.DeleteEvent, "Сотрудник {0} удалил ивент {1}" },
        
        { LogsEnum.CreateGame, "Сотрудник {0} создал игру {1} для ивента {2}" },
        { LogsEnum.EditGame, "Сотрудник {0} изменил игру {1} для ивента {2}" },
        { LogsEnum.DeleteGame, "Сотрудник {0} удалил игру {1} для ивента {2}" },
        
        { LogsEnum.CreateEmployee, "Сотрудник {0} создал сотрудника {1}" },
        { LogsEnum.DeleteEmployee, "Сотрудник {0} удалил сотрудника {1}" },
        { LogsEnum.RestoreEmployee, "Сотрудник {0} восстановил сотрудника {1}" },
        
        { LogsEnum.GiveCoins, "Сотрудник {0} выдал {1} монет пользователю {2}" },
    };
}