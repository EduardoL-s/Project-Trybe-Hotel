using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            User? userFound = _context.Users.FirstOrDefault(user => user.Email == email);
            Room? roomFound = _context.Rooms.FirstOrDefault(room => room.RoomId == booking.RoomId);
            Hotel? hotelFound = _context.Hotels.FirstOrDefault(hotel => hotel.HotelId == roomFound!.HotelId);
            City? cityFound = _context.Cities.FirstOrDefault(city => city.CityId == hotelFound!.CityId);

            if (roomFound!.Capacity < booking.GuestQuant) {
                return null!;
            }

            var newBooking = new Booking {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = userFound!.UserId,
                RoomId = booking.RoomId
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return new BookingResponse {
                BookingId = newBooking.BookingId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto {
                    RoomId = roomFound.RoomId,
                    Name = roomFound.Name,
                    Capacity = roomFound.Capacity,
                    Image = roomFound.Image,
                    Hotel = new HotelDto {
                        HotelId = hotelFound!.HotelId,
                        Name = hotelFound.Name,
                        Address = hotelFound.Address,
                        CityId = hotelFound.CityId,
                        CityName = cityFound!.Name,
                        State = cityFound.State
                    }
                }
            };
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            User? userFound = _context.Users.FirstOrDefault(user => user.Email == email);
            Booking? bookingFound = _context.Bookings.Find(bookingId);
            Room? roomFound = _context.Rooms.FirstOrDefault(room => room.RoomId == bookingFound!.RoomId);
            Hotel? hotelFound = _context.Hotels.FirstOrDefault(hotel => hotel.HotelId == roomFound!.HotelId);
            City? cityFound = _context.Cities.FirstOrDefault(city => city.CityId == hotelFound!.CityId);

            if (userFound == null || bookingFound == null || roomFound == null
                || hotelFound == null || cityFound == null
                || bookingFound.UserId != userFound.UserId) {
                return null!;
            }

            return new BookingResponse {
                BookingId = bookingFound.BookingId,
                CheckIn = bookingFound.CheckIn,
                CheckOut = bookingFound.CheckOut,
                GuestQuant = bookingFound.GuestQuant,
                Room = new RoomDto {
                    RoomId = roomFound.RoomId,
                    Name = roomFound.Name,
                    Capacity = roomFound.Capacity,
                    Image = roomFound.Image,
                    Hotel = new HotelDto {
                        HotelId = hotelFound!.HotelId,
                        Name = hotelFound.Name,
                        Address = hotelFound.Address,
                        CityId = hotelFound.CityId,
                        CityName = cityFound!.Name,
                        State = cityFound.State
                    }
                }
            };
        }

        public Room GetRoomById(int RoomId)
        {
             throw new NotImplementedException();
        }

    }

}