using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class Seed
    {
        public static async Task SeedDataAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (IsDatabaseEmpty<User>(dbContext))
                {
                    await SeedUsersAsync(dbContext);
                }

                if (IsDatabaseEmpty<Hotel>(dbContext))
                {
                    await SeedHotelsAsync(dbContext);
                }
                
                if (IsDatabaseEmpty<Comment>(dbContext))
                {
                    await SeedCommentsAsync(dbContext);
                }

                if (IsDatabaseEmpty<Room>(dbContext))
                {
                    await SeedRoomsAsync(dbContext);
                }

                if (IsDatabaseEmpty<Reservation>(dbContext))
                {
                    await SeedReservationsAsync(dbContext);
                }
            }
        }

        private static bool IsDatabaseEmpty<T>(DbContext context) where T : class
        {
            return !context.Set<T>().Any();
        }

        private static async Task SeedUsersAsync(ApplicationDbContext dbContext)
        {
            List<User> users = new List<User>
            {
                new() { Id = "1", Name = "Jan", LastName = "Kowalski", Email = "jan.kowalski@gmail.com" },
                new() { Id = "2", Name = "Anna", LastName = "Nowak", Email = "anna.nowak@yahoo.com" },
                new() { Id = "3", Name = "Michał", LastName = "Kowalski", Email = "michal.kowalski@outlook.com" },
                new() { Id = "4", Name = "Małgorzata", LastName = "Nowak", Email = "malgorzata.nowak@wp.pl" },
                new() { Id = "5", Name = "Krzysztof", LastName = "Kowalski", Email = "krzysztof.kowalski@onet.pl" },
                new() { Id = "6", Name = "Barbara", LastName = "Nowak", Email = "barbara.nowak@gmail.com" },
                new() { Id = "7", Name = "Andrzej", LastName = "Kowalski", Email = "andrzej.kowalski@yahoo.com" },
                new() { Id = "8", Name = "Elżbieta", LastName = "Nowak", Email = "elzbieta.nowak@outlook.com" },
                new() { Id = "9", Name = "Piotr", LastName = "Kowalski", Email = "piotr.kowalski@wp.pl" },
                new() { Id = "10", Name = "Wanda", LastName = "Nowak", Email = "wanda.nowak@onet.pl" },
                new() { Id = "11", Name = "Jerzy", LastName = "Kowalski", Email = "jerzy.kowalski@gmail.com" },
                new() { Id = "12", Name = "Halina", LastName = "Nowak", Email = "halina.nowak@yahoo.com" },
                new() { Id = "13", Name = "Marek", LastName = "Kowalski", Email = "marek.kowalski@outlook.com" },
                new() { Id = "14", Name = "Krystyna", LastName = "Nowak", Email = "krystyna.nowak@wp.pl" },
                new() { Id = "15", Name = "Tomasz", LastName = "Kowalski", Email = "tomasz.kowalski@onet.pl" },
                new() { Id = "16", Name = "Jolanta", LastName = "Nowak", Email = "jolanta.nowak@gmail.com" },
                new() { Id = "17", Name = "Wojciech", LastName = "Kowalski", Email = "wojciech.kowalski@yahoo.com" },
                new() { Id = "18", Name = "Danuta", LastName = "Nowak", Email = "danuta.nowak@outlook.com" },
                new() { Id = "19", Name = "Zbigniew", LastName = "Kowalski", Email = "zbigniew.kowalski@wp.pl" },
                new() { Id = "20", Name = "Genowefa", LastName = "Nowak", Email = "genowefa.nowak@onet.pl" }
            };

            await dbContext.AddRangeAsync(users);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedHotelsAsync(ApplicationDbContext dbContext)
        {
            var hotels = new List<Hotel>
            {
                new()
                {
                    Id = "1",
                    Name = "Hotel Nadmorski",
                    Description =
                        "Hotel Nadmorski to luksusowy ośrodek wypoczynkowy położony na wybrzeżu Bałtyku. Oferuje on swoim gościom eleganckie pokoje z widokiem na morze oraz bogatą gamę udogodnień. Hotel posiada spa i centrum odnowy biologicznej, w którym można skorzystać z masaży, sauny i jacuzzi. Restauracja serwuje wykwintne dania kuchni międzynarodowej, a także lokalne specjały. Hotel Nadmorski to idealne miejsce dla osób poszukujących relaksu i wspaniałych widoków na morze.",
                    Address = "ul. Nadmorska 1",
                    City = "Gdańsk",
                    PhoneNumber = "+48 123 456 789",
                    Stars = 5,
                    Category = "Hotel"
                },
                new()
                {
                    Id = "2",
                    Name = "Hotel Górski Raj",
                    Description =
                        "Hotel Górski Raj to malowniczo położony ośrodek wypoczynkowy w sercu polskich gór. Otoczony piękną przyrodą, oferuje on swoim gościom komfortowe pokoje z widokiem na górskie szczyty. Hotel posiada własne stoki narciarskie oraz szkółkę nauki jazdy na nartach. Po całym dniu aktywności na świeżym powietrzu można zrelaksować się w centrum spa, które oferuje różnorodne zabiegi odnowy biologicznej. Restauracja serwuje tradycyjne dania regionalne oraz międzynarodowe specjały. Hotel Górski Raj to doskonałe miejsce dla miłośników górskich przygód.",
                    Address = "ul. Górska 2",
                    City = "Zakopane",
                    PhoneNumber = "+48 987 654 321",
                    Stars = 4,
                    Category = "Hostel"
                },
                new()
                {
                    Id = "3",
                    Name = "Hotel Dworski",
                    Description =
                        "Hotel Dworski to elegancki dworek zlokalizowany w urokliwej polskiej wsi. Zbudowany w tradycyjnym stylu, oferuje on swoim gościom przytulne pokoje urządzone w rustykalnym klimacie. Hotel posiada piękny ogród, w którym można odpocząć i cieszyć się ciszą i spokojem. Restauracja serwuje dania kuchni polskiej przygotowane ze świeżych, lokalnych produktów. Wokół hotelu znajduje się wiele atrakcji, takich jak malownicze trasy rowerowe i szlaki turystyczne. Hotel Dworski to doskonałe miejsce dla osób pragnących oderwać się od miejskiego zgiełku i zanurzyć w wiejskiej atmosferze.",
                    Address = "ul. Wiejska 3",
                    City = "Kazimierz Dolny",
                    PhoneNumber = "+48 111 222 333",
                    Stars = 3,
                    Category = "Luxury Hotel"
                },
                new()
                {
                    Id = "4",
                    Name = "Hotel Nowoczesny",
                    Description =
                        "Hotel Nowoczesny to designerski hotel zlokalizowany w samym centrum Warszawy. Jego nowoczesna architektura i stylowe wnętrza zachwycą nawet najbardziej wymagających gości. Oferuje on wygodne pokoje z nowoczesnymi udogodnieniami, takimi jak telewizory z płaskim ekranem i bezpłatne WiFi. Hotel posiada również centrum fitness oraz trendy bar, w którym serwowane są unikalne drinki i przekąski. Dzięki swojemu centralnemu położeniu, hotel stanowi doskonałą bazę do zwiedzania stolicy Polski.",
                    Address = "ul. Nowoczesna 4",
                    City = "Warszawa",
                    PhoneNumber = "+48 555 666 777",
                    Stars = 4,
                    Category = "Hostel"
                },
                new()
                {
                    Id = "5",
                    Name = "Hotel Spa & Wellness",
                    Description =
                        "Hotel Spa & Wellness to luksusowy kompleks hotelowy położony nad jeziorem. Oferuje on swoim gościom eleganckie apartamenty z widokiem na wodę oraz bogate zaplecze wellness. Hotel posiada rozległy kompleks basenów, saun i łaźni parowych, które zapewniają doskonały relaks. W spa dostępne są również różnorodne zabiegi kosmetyczne i masaże. Restauracja serwuje dania kuchni zdrowej oraz międzynarodowej. Hotel Spa & Wellness to idealne miejsce dla osób pragnących odzyskać równowagę i w pełni zrelaksować się w otoczeniu pięknej przyrody.",
                    Address = "ul. Wellnessowa 5",
                    City = "Międzyzdroje",
                    PhoneNumber = "+48 777 888 999",
                    Stars = 5,
                    Category = "Hostel"
                }
            };

            await dbContext.AddRangeAsync(hotels);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedCommentsAsync(ApplicationDbContext dbContext)
        {
            var comments = new List<Comment>
            {
                new()
                {
                    Id = "1",
                    Description =
                        "Mieliśmy niesamowite doświadczenie w tym hotelu. Personel był niezwykle przyjazny i pomocny, a pokoje przestronne i pięknie urządzone. Lokalizacja była również idealna, z łatwym dostępem do popularnych atrakcji i restauracji. Bardzo polecam!",
                    UserId = "1",
                    HotelId = "1"
                },
                new()
                {
                    Id = "2",
                    Description =
                        "Nie mogę wystarczająco polecić tego hotelu. Od momentu przybycia zostaliśmy powitani ciepłym uśmiechem i wyjątkową obsługą. Pokój był czysty, wygodny i miał przepiękny widok na miasto. Udogodnienia hotelowe były na najwyższym poziomie, a restauracja serwowała pyszne posiłki. Na pewno tu wrócę!",
                    UserId = "2",
                    HotelId = "1"
                },
                new()
                {
                    Id = "3",
                    Description =
                        "Podczas pobytu w hotelu napotkaliśmy kilka problemów, pomimo świetnej lokalizacji i ładnych pokoi. Klimatyzacja nie działała poprawnie, a personel wydawał się przytłoczony i nieodpowiednio reagował na nasze uwagi. Szkoda, bo hotel ma potencjał, ale doświadczenie okazało się nieco poniżej oczekiwań.",
                    UserId = "3",
                    HotelId = "2"
                },
                new()
                {
                    Id = "4",
                    Description =
                        "Naprawdę świetnie się bawiliśmy w tym hotelu. Personel zrobił wszystko, aby zapewnić nam komfort i zadowolenie. Pokój był czysty i dobrze wyposażony, a łóżko niezwykle wygodne. Udogodnienia hotelu, takie jak basen i centrum fitness, były również fantastyczne. Nie mogę się doczekać kolejnej wizyty!",
                    UserId = "4",
                    HotelId = "2"
                },
                new()
                {
                    Id = "5",
                    Description =
                        "Ten hotel przekroczył nasze oczekiwania pod każdym względem. Od momentu wejścia do holu zostaliśmy przywitani przez przyjazny personel, który zapewnił wyjątkową obsługę przez cały nasz pobyt. Pokój był przestronny, gustownie urządzony i wyposażony we wszystkie niezbędne udogodnienia. Lokalizacja hotelu była doskonała, z łatwym dostępem do sklepów i restauracji. Gorąco polecam!",
                    UserId = "5",
                    HotelId = "3"
                },
                new()
                {
                    Id = "6",
                    Description =
                        "Mieliśmy fantastyczny czas w tym hotelu. Pokój był czysty, wygodny i miał zapierający dech w piersiach widok na ocean. Personel hotelu był przyjazny i troskliwy, dbając o to, aby wszystkie nasze potrzeby zostały zaspokojone. Restauracja serwowała dania kuchni zdrowej oraz międzynarodowej. Hotel Spa & Wellness to idealne miejsce dla osób pragnących odzyskać równowagę i w pełni zrelaksować się w otoczeniu pięknej przyrody.",
                    UserId = "6",
                    HotelId = "3"
                },
                new()
                {
                    Id = "7",
                    Description =
                        "Niestety, mój pobyt w tym hotelu był rozczarowujący. Pokój był nieczyści, a personel był nieuprzejmy i mało pomocny. Niezadowolony z obsługi i ogólnego doświadczenia.",
                    UserId = "7",
                    HotelId = "4"
                },
                new()
                {
                    Id = "8",
                    Description =
                        "Bardzo miło wspominam pobyt w tym hotelu. Pokój był czysty i wygodny, a personel był przyjazny i profesjonalny. Śniadanie było smaczne i urozmaicone. Lokalizacja hotelu była dogodna, blisko do atrakcji turystycznych. Gorąco polecam!",
                    UserId = "8",
                    HotelId = "4"
                },
                new()
                {
                    Id = "9",
                    Description =
                        "Ten hotel to prawdziwa perełka. Od momentu przybycia zostaliśmy przywitani z uśmiechem i serdecznością. Pokój był luksusowy, z widokiem na morze i wszystkimi wygodami, których można sobie wymarzyć. Personel był niezwykle pomocny i profesjonalny. Wszystko było doskonałe, od jedzenia w restauracji po udogodnienia w spa. Na pewno wrócimy tu ponownie!",
                    UserId = "9",
                    HotelId = "5"
                },
                new()
                {
                    Id = "10",
                    Description =
                        "Mój pobyt w tym hotelu był niezapomniany. Wszystko było perfekcyjne, od pokoju, który był przestronny i pięknie urządzony, po obsługę, która była uprzejma i przyjazna. Restauracja serwowała wyśmienite jedzenie, a basen był idealnym miejscem do relaksu. Polecam ten hotel każdemu, kto szuka luksusowego doświadczenia.",
                    UserId = "10",
                    HotelId = "5"
                },
                new()
                {
                    Id = "11",
                    Description =
                        "Hotel ten przeszedł nasze najśmielsze oczekiwania. Pokój był przepiękny, czysty i oferował piękne widoki na okolicę. Personel był niezwykle uprzejmy i profesjonalny, zawsze gotowy do pomocy. Restauracja w hotelu serwowała pyszne jedzenie, a basen był doskonały do relaksu. Zdecydowanie polecam!",
                    UserId = "11",
                    HotelId = "1"
                },
                new()
                {
                    Id = "12",
                    Description =
                        "Spędziliśmy niezapomniany weekend w tym hotelu. Wszystko było perfekcyjne - od czystości pokoi po obsługę kelnerską w restauracji. Personel jest wyjątkowo przyjazny i uprzejmy, zawsze starający się spełnić nasze oczekiwania. Lokalizacja hotelu jest również świetna, blisko atrakcji turystycznych. Nie możemy się doczekać powrotu!",
                    UserId = "12",
                    HotelId = "1"
                },
                new()
                {
                    Id = "13",
                    Description =
                        "Ten hotel to prawdziwa perełka. Pokoje są przestronne, elegancko urządzone i oferują niesamowite widoki na okolicę. Personel jest bardzo profesjonalny i zawsze chętny do pomocy. Restauracja serwuje wyśmienite dania, a spa zapewnia relaks i odprężenie. Gorąco polecam to miejsce!",
                    UserId = "13",
                    HotelId = "2"
                },
                new()
                {
                    Id = "14",
                    Description =
                        "Nasz pobyt w tym hotelu był absolutnie fantastyczny. Pokój był luksusowy, czysty i przestronny, a personel dbał o każdy detal. Restauracja serwowała wyborne jedzenie, a obsługa była bardzo przyjazna. Hotel ma także świetne udogodnienia, w tym basen i siłownię. Na pewno tu wrócimy!",
                    UserId = "14",
                    HotelId = "2"
                },
                new()
                {
                    Id = "15",
                    Description =
                        "To miejsce to prawdziwy raj dla miłośników luksusu. Pokoje są pięknie urządzone, z dbałością o każdy detal. Personel jest wyjątkowo profesjonalny i uprzejmy, a obsługa jest na najwyższym poziomie. Restauracja serwuje wyśmienite dania, a spa oferuje szeroki wybór zabiegów. Nie możemy się doczekać kolejnej wizyty!",
                    UserId = "15",
                    HotelId = "2"
                },
                new()
                {
                    Id = "16",
                    Description =
                        "Spędziliśmy tu romantyczny weekend i było to absolutnie magiczne. Pokój był piękny, z pięknym widokiem i luksusowym wyposażeniem. Personel był niezwykle troskliwy i zawsze gotowy do pomocy. Restauracja serwowała przepyszne dania, a atmosfera była niezwykle romantyczna. To naprawdę niezapomniane miejsce!",
                    UserId = "16",
                    HotelId = "2"
                },
                new()
                {
                    Id = "17",
                    Description =
                        "Ten hotel to doskonały wybór dla każdego podróżującego. Pokoje są czyste, wygodne i oferują wszystko, czego potrzebujesz podczas pobytu. Personel jest niezwykle przyjazny i pomocny, zawsze gotowy odpowiedzieć na pytania i udzielić wskazówek dotyczących zwiedzania okolicy. Gorąco polecam!",
                    UserId = "17",
                    HotelId = "3"
                },
                new()
                {
                    Id = "18",
                    Description =
                        "Nasz pobyt w tym hotelu był niezwykle udany. Pokój był przestronny, czysty i wygodny, a personel był niezwykle uprzejmy i profesjonalny. Restauracja w hotelu serwowała pyszne jedzenie, a obsługa była bez zarzutu. Polecam to miejsce wszystkim, którzy szukają komfortu i wyjątkowej obsługi.",
                    UserId = "18",
                    HotelId = "3"
                },
                new()
                {
                    Id = "19",
                    Description =
                        "To miejsce jest prawdziwym klejnotem. Pokoje są gustownie urządzone, wygodne i czyste. Personel jest przyjazny i zawsze stara się spełnić oczekiwania gości. Restauracja serwuje wyborne dania, a spa oferuje doskonałe zabiegi relaksacyjne. To idealne miejsce na wypoczynek!",
                    UserId = "19",
                    HotelId = "2"
                },
                new()
                {
                    Id = "20",
                    Description =
                        "Spędziliśmy tu wspaniałe wakacje. Hotel ma świetną lokalizację i zapewniał nam niesamowity widok na góry. Pokoje są przestronne i czyste, a personel jest przyjazny i pomocny. Restauracja serwuje pyszne posiłki, a dodatkowe udogodnienia, takie jak basen i sauna, sprawiają, że pobyt był jeszcze bardziej relaksujący. Na pewno tu wrócimy!",
                    UserId = "20",
                    HotelId = "2"
                },
            };

            await dbContext.Comments.AddRangeAsync(comments);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedReservationsAsync(ApplicationDbContext dbContext)
        {
            var reservations = new List<Reservation>
            {
                new()
                {
                    Id = "1",
                    CheckInDate = DateTimeOffset.Now.AddDays(2),
                    CheckOutDate = DateTimeOffset.Now.AddDays(5),
                    HotelId = "1",
                    RoomId = "1",
                    UserId = "1"
                },
                new()
                {
                    Id = "2",
                    CheckInDate = DateTimeOffset.Now.AddDays(7),
                    CheckOutDate = DateTimeOffset.Now.AddDays(10),
                    HotelId = "2",
                    RoomId = "2",
                    UserId = "2"
                },
                new()
                {
                    Id = "3",
                    CheckInDate = DateTimeOffset.Now.AddDays(12),
                    CheckOutDate = DateTimeOffset.Now.AddDays(15),
                    HotelId = "3",
                    RoomId = "3",
                    UserId = "3"
                },
                new()
                {
                    Id = "4",
                    CheckInDate = DateTimeOffset.Now.AddDays(18),
                    CheckOutDate = DateTimeOffset.Now.AddDays(21),
                    HotelId = "4",
                    RoomId = "4",
                    UserId = "4"
                },
                new()
                {
                    Id = "5",
                    CheckInDate = DateTimeOffset.Now.AddDays(25),
                    CheckOutDate = DateTimeOffset.Now.AddDays(28),
                    HotelId = "5",
                    RoomId = "5",
                    UserId = "5"
                },
                new()
                {
                    Id = "6",
                    CheckInDate = DateTimeOffset.Now.AddDays(3),
                    CheckOutDate = DateTimeOffset.Now.AddDays(8),
                    HotelId = "1",
                    RoomId = "1",
                    UserId = "6"
                },
                new()
                {
                    Id = "7",
                    CheckInDate = DateTimeOffset.Now.AddDays(10),
                    CheckOutDate = DateTimeOffset.Now.AddDays(13),
                    HotelId = "2",
                    RoomId = "2",
                    UserId = "7"
                },
                new()
                {
                    Id = "8",
                    CheckInDate = DateTimeOffset.Now.AddDays(17),
                    CheckOutDate = DateTimeOffset.Now.AddDays(20),
                    HotelId = "3",
                    RoomId = "3",
                    UserId = "8"
                },
                new()
                {
                    Id = "9",
                    CheckInDate = DateTimeOffset.Now.AddDays(24),
                    CheckOutDate = DateTimeOffset.Now.AddDays(27),
                    HotelId = "4",
                    RoomId = "4",
                    UserId = "9"
                },
                new()
                {
                    Id = "10",
                    CheckInDate = DateTimeOffset.Now.AddDays(30),
                    CheckOutDate = DateTimeOffset.Now.AddDays(33),
                    HotelId = "5",
                    RoomId = "5",
                    UserId = "10"
                },
                new()
                {
                    Id = "11",
                    CheckInDate = DateTimeOffset.Now.AddDays(4),
                    CheckOutDate = DateTimeOffset.Now.AddDays(9),
                    HotelId = "1",
                    RoomId = "1",
                    UserId = "11"
                },
                new()
                {
                    Id = "12",
                    CheckInDate = DateTimeOffset.Now.AddDays(11),
                    CheckOutDate = DateTimeOffset.Now.AddDays(14),
                    HotelId = "2",
                    RoomId = "2",
                    UserId = "12"
                },
                new()
                {
                    Id = "13",
                    CheckInDate = DateTimeOffset.Now.AddDays(19),
                    CheckOutDate = DateTimeOffset.Now.AddDays(22),
                    HotelId = "3",
                    RoomId = "3",
                    UserId = "13"
                },
                new()
                {
                    Id = "14",
                    CheckInDate = DateTimeOffset.Now.AddDays(26),
                    CheckOutDate = DateTimeOffset.Now.AddDays(29),
                    HotelId = "4",
                    RoomId = "4",
                    UserId = "14"
                },
                new()
                {
                    Id = "15",
                    CheckInDate = DateTimeOffset.Now.AddDays(5),
                    CheckOutDate = DateTimeOffset.Now.AddDays(10),
                    HotelId = "5",
                    RoomId = "5",
                    UserId = "15"
                },
                new()
                {
                    Id = "16",
                    CheckInDate = DateTimeOffset.Now.AddDays(13),
                    CheckOutDate = DateTimeOffset.Now.AddDays(18),
                    HotelId = "1",
                    RoomId = "1",
                    UserId = "16"
                },
                new()
                {
                    Id = "17",
                    CheckInDate = DateTimeOffset.Now.AddDays(21),
                    CheckOutDate = DateTimeOffset.Now.AddDays(24),
                    HotelId = "2",
                    RoomId = "2",
                    UserId = "17"
                },
                new()
                {
                    Id = "18",
                    CheckInDate = DateTimeOffset.Now.AddDays(28),
                    CheckOutDate = DateTimeOffset.Now.AddDays(31),
                    HotelId = "3",
                    RoomId = "3",
                    UserId = "18"
                },
                new()
                {
                    Id = "19",
                    CheckInDate = DateTimeOffset.Now.AddDays(6),
                    CheckOutDate = DateTimeOffset.Now.AddDays(11),
                    HotelId = "4",
                    RoomId = "4",
                    UserId = "19"
                },
                new()
                {
                    Id = "20",
                    CheckInDate = DateTimeOffset.Now.AddDays(14),
                    CheckOutDate = DateTimeOffset.Now.AddDays(19),
                    HotelId = "5",
                    RoomId = "5",
                    UserId = "20"
                }
            };
            await dbContext.AddRangeAsync(reservations);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedRoomsAsync(ApplicationDbContext dbContext)
        {
            List<Room> rooms = new List<Room>
            {
                new()
                {
                    Id = "1",
                    Capacity = 2,
                    Name = "Pokój standardowy",
                    Description =
                        "Przytulny pokój z łóżkiem podwójnym i łazienką. Wyposażony w telewizor, szafę i biurko. Idealny dla par lub podróżujących w pojedynkę. Usługa sprzątania codziennie.",
                    Price = 200,
                    HotelId = "1"
                },
                new()
                {
                    Id = "2",
                    Capacity = 1,
                    Name = "Pokój jednoosobowy",
                    Description =
                        "Komfortowy pokój dla jednej osoby z prywatną łazienką. Wyposażony w wygodne łóżko, telewizor, biurko i szafę. Doskonały dla osób podróżujących służbowo lub indywidualnie. Bezpłatny dostęp do WiFi.",
                    Price = 150,
                    HotelId = "1"
                },
                new()
                {
                    Id = "3",
                    Capacity = 4,
                    Name = "Apartament rodzinny",
                    Description =
                        "Przestronny apartament z dwoma sypialniami, kuchnią i łazienką. Wyposażony w telewizory w obu sypialniach i salonie, jadalnię oraz zestaw do parzenia kawy i herbaty. Doskonały dla rodzin lub grupy przyjaciół. Bezpłatny parking dla gości.",
                    Price = 400,
                    HotelId = "2"
                },
                new()
                {
                    Id = "4",
                    Capacity = 2,
                    Name = "Pokój deluxe",
                    Description =
                        "Elegancki pokój z panoramicznym widokiem na miasto i prywatnym jacuzzi. Wyposażony w wygodne łóżko, telewizor, minibar oraz luksusową łazienkę. Dodatkowo oferuje bezpłatne śniadanie w pokoju. Dostęp do centrum fitness i sauny.",
                    Price = 350,
                    HotelId = "2"
                },
                new()
                {
                    Id = "5",
                    Capacity = 3,
                    Name = "Pokój deluxe",
                    Description =
                        "Luksusowy pokój z dodatkowym przestrzenią do pracy i relaksu. Wyposażony w wygodne łóżka, telewizor, biurko, sofę oraz kawę i herbatę. Zapewniamy codzienny serwis sprzątający oraz dostęp do restauracji i barów w hotelu.",
                    Price = 300,
                    HotelId = "3"
                }
            };
            await dbContext.AddRangeAsync(rooms);
            await dbContext.SaveChangesAsync();
        }
    }
}