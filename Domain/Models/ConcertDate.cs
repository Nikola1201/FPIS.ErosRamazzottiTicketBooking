using System.Net.Sockets;

namespace FPIS.Domain.Models;

public class ConcertDate
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Guid ConcertId { get; set; }
    public Concert Concert { get; set; } = new();
    public ICollection<ReservationTicket> Tickets { get; set; } = new List<ReservationTicket>();
}