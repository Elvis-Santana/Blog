using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Notification
{
    public string Id { get; init; }
    public string UserId { get; init; }
    public string Messsage { get; init; }
    public bool IsRead { get; private set; } = default(bool);


    protected Notification() { }

    private Notification(string UserId,string Messsage)
    {
        this.Id = Guid.NewGuid().ToString();
        this.UserId = UserId;
        this.Messsage = Messsage;
    }

    public void NotificationIsRead() => IsRead = true;

    public static Notification Create(string UserId, string Messsage) => new(UserId,Messsage);

}
