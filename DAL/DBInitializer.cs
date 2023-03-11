using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using GoodJoker.Models;

namespace GoodJoker.DAL
{
    public class DBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            var roles = new List<Role>();
            var studRoles = new List<StudioRoleMember>();

            var role1 = new Role { Name = "Admin" };
            var role2 = new Role { Name = "User" };
            roles.Add(role1);
            roles.Add(role2);

            var roleStudio = new StudioRoleMember { Name = "Creator" };

            var defaultCity = new City { Name = "нет", Index = 0, Region = null };
            db.Cities.Add(defaultCity);

            var reg = true;
            if (reg)
            {
                var regions = new List<RegionCity>();

                var region1 = new RegionCity() { Name = "Ханты-Мансийский автономный округ - Югра" };
                    region1.Cities.Add(new City() { Index = 628400, Name = "г. Сургут" });
                    region1.Cities.Add(new City() { Index = 628433, Name = "п.г.т. Белый Яр" });
                    region1.Cities.Add(new City() { Index = 628160, Name = "г. Белоярский" });
                    region1.Cities.Add(new City() { Index = 628481, Name = "г. Когалым" });
                    region1.Cities.Add(new City() { Index = 628672, Name = "г. Лангепас" });
                    region1.Cities.Add(new City() { Index = 628449, Name = "г. Лянтор" });
                    region1.Cities.Add(new City() { Index = 628680, Name = "г. Мегион" });
                    region1.Cities.Add(new City() { Index = 628300, Name = "г. Нефтеюганск" });
                    region1.Cities.Add(new City() { Index = 628600, Name = "г. Нижневартовск" });
                    region1.Cities.Add(new City() { Index = 628181, Name = "г. Нягань" });
                    region1.Cities.Add(new City() { Index = 628661, Name = "г. Покачи" });
                    region1.Cities.Add(new City() { Index = 628380, Name = "г. Пыть-Ях" });
                    region1.Cities.Add(new City() { Index = 628461, Name = "г. Радужный" });
                    region1.Cities.Add(new City() { Index = 628240, Name = "г. Советский" });
                    region1.Cities.Add(new City() { Index = 628285, Name = "г. Урай" });
                    region1.Cities.Add(new City() { Index = 628000, Name = "г. Ханты-Мансийск" });
                    region1.Cities.Add(new City() { Index = 628260, Name = "г. Югорск" });
                    regions.Add(region1);

                var region2 = new RegionCity { Name = "Республика Хакасия" };
                    region2.Cities.Add(new City { Index = 655750, Name = "г. Абаза" });
                    region2.Cities.Add(new City { Index = 655017, Name = "г. Абакан" });
                    region2.Cities.Add(new City { Index = 655602, Name = "г. Саяногорск" });
                    region2.Cities.Add(new City { Index = 655111, Name = "г. Сорск" });
                    region2.Cities.Add(new City { Index = 655150, Name = "г. Черногорск" });
                    regions.Add(region2);

                var region3 = new RegionCity { Name = "Оренбургская область" };
                    region3.Cities.Add(new City { Index = 461743, Name = "г. Абдулино" });
                    region3.Cities.Add(new City { Index = 461630, Name = "г. Бугуруслан" });
                    region3.Cities.Add(new City { Index = 461040, Name = "г. Бузулук" });
                    region3.Cities.Add(new City { Index = 462630, Name = "г. Гай" });
                    region3.Cities.Add(new City { Index = 462240, Name = "г. Кувандык" });
                    region3.Cities.Add(new City { Index = 462274, Name = "г. Медногорск" });
                    region3.Cities.Add(new City { Index = 462351, Name = "г. Новотроицк" });
                    region3.Cities.Add(new City { Index = 460000, Name = "г. Оренбург" });
                    region3.Cities.Add(new City { Index = 462400, Name = "г. Орск" });
                    region3.Cities.Add(new City { Index = 461501, Name = "г. Соль-Илецк" });
                    region3.Cities.Add(new City { Index = 461900, Name = "г. Сорочинск" });
                    region3.Cities.Add(new City { Index = 462781, Name = "г. Ясный" });
                    regions.Add(region3);

                var region4 = new RegionCity { Name = "Краснодарский край" };
                    region4.Cities.Add(new City { Index = 353320, Name = "г. Абинск" });
                    region4.Cities.Add(new City { Index = 353440, Name = "г. Анапа" });
                    region4.Cities.Add(new City { Index = 352690, Name = "г. Апшеронск" });
                    region4.Cities.Add(new City { Index = 352900, Name = "г. Армавир" });
                    region4.Cities.Add(new City { Index = 352630, Name = "г. Белореченск" });
                    region4.Cities.Add(new City { Index = 353460, Name = "г. Геленджик" });
                    region4.Cities.Add(new City { Index = 353290, Name = "г. Горячий Ключ" });
                    region4.Cities.Add(new City { Index = 352190, Name = "г. Гулькевичи" });
                    region4.Cities.Add(new City { Index = 353680, Name = "г. Ейск" });
                    region4.Cities.Add(new City { Index = 353180, Name = "г. Кореновск" });
                    region4.Cities.Add(new City { Index = 350000, Name = "г. Краснодар" });
                    region4.Cities.Add(new City { Index = 352380, Name = "г. Кропоткин" });
                    region4.Cities.Add(new City { Index = 353380, Name = "г. Крымск" });
                    region4.Cities.Add(new City { Index = 352430, Name = "г. Курганинск" });
                    region4.Cities.Add(new City { Index = 352500, Name = "г. Лабинск" });
                    region4.Cities.Add(new City { Index = 352240, Name = "г. Новокубанск" });
                    region4.Cities.Add(new City { Index = 353900, Name = "г. Новороссийск" });
                    region4.Cities.Add(new City { Index = 353860, Name = "г. Приморско-Ахтарск" });
                    region4.Cities.Add(new City { Index = 353560, Name = "г. Славянск-на-Кубани" });
                    region4.Cities.Add(new City { Index = 354000, Name = "г. Сочи" });
                    region4.Cities.Add(new City { Index = 353500, Name = "г. Темрюк" });
                    region4.Cities.Add(new City { Index = 352700, Name = "г. Тимашевск" });
                    region4.Cities.Add(new City { Index = 352120, Name = "г. Тихорецк" });
                    region4.Cities.Add(new City { Index = 352800, Name = "г. Туапсе" });
                    region4.Cities.Add(new City { Index = 352330, Name = "г. Усть-Лабинск" });
                    region4.Cities.Add(new City { Index = 352680, Name = "г. Хадыженск" });
                    regions.Add(region4);

                var region5 = new RegionCity { Name = "Республика Башкортостан" };
                    region5.Cities.Add(new City { Index = 452920, Name = "г. Агидель" });
                    region5.Cities.Add(new City { Index = 453630, Name = "г. Баймак" });
                    region5.Cities.Add(new City { Index = 452030, Name = "г. Белебей" });
                    region5.Cities.Add(new City { Index = 453500, Name = "г. Белорецк" });
                    region5.Cities.Add(new City { Index = 452450, Name = "г. Бирск" });
                    region5.Cities.Add(new City { Index = 453400, Name = "г. Давлеканово" });
                    region5.Cities.Add(new City { Index = 452320, Name = "г. Дюртюли" });
                    region5.Cities.Add(new City { Index = 453200, Name = "г. Ишимбай" });
                    region5.Cities.Add(new City { Index = 453300, Name = "г. Кумертау" });
                    region5.Cities.Add(new City { Index = 453570, Name = "г. Межгорье" });
                    region5.Cities.Add(new City { Index = 453850, Name = "г. Мелеуз" });
                    region5.Cities.Add(new City { Index = 452680, Name = "г. Нефтекамск" });
                    region5.Cities.Add(new City { Index = 452600, Name = "г. Октябрьский" });
                    region5.Cities.Add(new City { Index = 453250, Name = "г. Салават" });
                    region5.Cities.Add(new City { Index = 453833, Name = "г. Сибай" });
                    region5.Cities.Add(new City { Index = 453100, Name = "г. Стерлитамак" });
                    region5.Cities.Add(new City { Index = 452750, Name = "г. Туймазы" });
                    region5.Cities.Add(new City { Index = 450000, Name = "г. Уфа" });
                    region5.Cities.Add(new City { Index = 453700, Name = "г. Учалы" });
                    region5.Cities.Add(new City { Index = 452800, Name = "г. Янаул" });
                    regions.Add(region5);

                var region6 = new RegionCity { Name = "Республика Татарстан" };
                    region6.Cities.Add(new City { Index = 422230, Name = "г. Агрыз" });
                    region6.Cities.Add(new City { Index = 423330, Name = "г. Азнакаево" });
                    region6.Cities.Add(new City { Index = 423450, Name = "г. Альметьевск" });
                    region6.Cities.Add(new City { Index = 423930, Name = "г. Бавлы" });
                    region6.Cities.Add(new City { Index = 422840, Name = "г. Болгар" });
                    region6.Cities.Add(new City { Index = 423200, Name = "г. Бугульма" });
                    region6.Cities.Add(new City { Index = 422430, Name = "г. Буинск" });
                    region6.Cities.Add(new City { Index = 423600, Name = "г. Елабуга" });
                    region6.Cities.Add(new City { Index = 423520, Name = "г. Заинск" });
                    region6.Cities.Add(new City { Index = 422540, Name = "г. Зеленодольск" });
                    region6.Cities.Add(new City { Index = 420000, Name = "г. Казань" });
                    region6.Cities.Add(new City { Index = 423250, Name = "г. Лениногорск" });
                    region6.Cities.Add(new City { Index = 422190, Name = "г. Мамадыш" });
                    region6.Cities.Add(new City { Index = 423650, Name = "г. Менделеевск" });
                    region6.Cities.Add(new City { Index = 423700, Name = "г. Мензелинск" });
                    region6.Cities.Add(new City { Index = 423800, Name = "г. Набережные Челны" });
                    region6.Cities.Add(new City { Index = 423585, Name = "г. Нижнекамск" });
                    region6.Cities.Add(new City { Index = 423040, Name = "г. Нурлат" });
                    region6.Cities.Add(new City { Index = 422370, Name = "г. Тетюши" });
                    region6.Cities.Add(new City { Index = 422980, Name = "г. Чистополь" });
                    regions.Add(region6);

                var region7 = new RegionCity { Name = "Республика Адыгея" };
                    region7.Cities.Add(new City { Index = 385200, Name = "г. Адыгейск" });
                    region7.Cities.Add(new City { Index = 385000, Name = "г. Майкоп" });
                    regions.Add(region7);

                var region8 = new RegionCity { Name = "Ростовская область" };
                    region8.Cities.Add(new City { Index = 346780, Name = "г. Азов" });
                    region8.Cities.Add(new City { Index = 346720, Name = "г. Аксай" });
                    region8.Cities.Add(new City { Index = 346880, Name = "г. Батайск" });
                    region8.Cities.Add(new City { Index = 347040, Name = "г. Белая Калитва" });
                    region8.Cities.Add(new City { Index = 347340, Name = "г. Волгодонск" });
                    region8.Cities.Add(new City { Index = 347880, Name = "г. Гуково" });
                    region8.Cities.Add(new City { Index = 346330, Name = "г. Донецк" });
                    region8.Cities.Add(new City { Index = 346310, Name = "г. Зверево" });
                    region8.Cities.Add(new City { Index = 347740, Name = "г. Зерноград" });
                    region8.Cities.Add(new City { Index = 347800, Name = "г. Каменск-Шахтинский" });
                    region8.Cities.Add(new City { Index = 347250, Name = "г. Константиновск" });
                    region8.Cities.Add(new City { Index = 346350, Name = "г. Красный Сулин" });
                    region8.Cities.Add(new City { Index = 346130, Name = "г. Миллерово" });
                    region8.Cities.Add(new City { Index = 347210, Name = "г. Морозовск" });
                    region8.Cities.Add(new City { Index = 346400, Name = "г. Новочеркасск" });
                    region8.Cities.Add(new City { Index = 346900, Name = "г. Новошахтинск" });
                    region8.Cities.Add(new City { Index = 347540, Name = "г. Пролетарск" });
                    region8.Cities.Add(new City { Index = 344000, Name = "г. Ростов-на-Дону" });
                    region8.Cities.Add(new City { Index = 347630, Name = "г. Сальск" });
                    region8.Cities.Add(new City { Index = 346630, Name = "г. Семикаракорск" });
                    region8.Cities.Add(new City { Index = 347900, Name = "г. Таганрог" });
                    region8.Cities.Add(new City { Index = 347320, Name = "г. Цимлянск" });
                    region8.Cities.Add(new City { Index = 346500, Name = "г. Шахты" });
                    regions.Add(region8);

                var region9 = new RegionCity { Name = "Республика Тыва" };
                    region9.Cities.Add(new City { Index = 668050, Name = "г. Ак-Довурак" });
                    region9.Cities.Add(new City { Index = 667001, Name = "г. Кызыл" });
                    region9.Cities.Add(new City { Index = 668510, Name = "г. Туран" });
                    region9.Cities.Add(new City { Index = 668110, Name = "г. Чадан" });
                    region9.Cities.Add(new City { Index = 668210, Name = "г. Шагонар" });
                    regions.Add(region9);

                var region10 = new RegionCity { Name = "Республика Северная Осетия" };
                    region10.Cities.Add(new City { Index = 363240, Name = "г. Алагир" });
                    region10.Cities.Add(new City { Index = 363330, Name = "г. Ардон" });
                    region10.Cities.Add(new City { Index = 363020, Name = "г. Беслан" });
                    region10.Cities.Add(new City { Index = 362000, Name = "г. Владикавказ" });
                    region10.Cities.Add(new City { Index = 363410, Name = "г. Дигора" });
                    region10.Cities.Add(new City { Index = 363750, Name = "г. Моздок" });
                    regions.Add(region10);

                var region11 = new RegionCity { Name = "Свердловская область" };
                    region11.Cities.Add(new City { Index = 624600, Name = "г. Алапаевск" });
                    region11.Cities.Add(new City { Index = 624000, Name = "г. Арамиль" });
                    region11.Cities.Add(new City { Index = 623780, Name = "г. Артёмовский" });
                    region11.Cities.Add(new City { Index = 624260, Name = "г. Азбест" });
                    region11.Cities.Add(new City { Index = 623700, Name = "г. Берёзовский" });
                    region11.Cities.Add(new City { Index = 623530, Name = "г. Богданович" });
                    region11.Cities.Add(new City { Index = 624162, Name = "г. Верхний Тагил" });
                    region11.Cities.Add(new City { Index = 624090, Name = "г. Верхняя Пышма" });
                    region11.Cities.Add(new City { Index = 624760, Name = "г. Верхняя Салда" });
                    region11.Cities.Add(new City { Index = 624320, Name = "г. Верхняя Тура" });
                    region11.Cities.Add(new City { Index = 624380, Name = "г. Верхотурье" });
                    region11.Cities.Add(new City { Index = 624941, Name = "г. Волчанск" });
                    region11.Cities.Add(new City { Index = 623270, Name = "г. Дегтярск" });
                    region11.Cities.Add(new City { Index = 620000, Name = "г. Екатеринбург" });
                    region11.Cities.Add(new City { Index = 624250, Name = "г. Заречный" });
                    region11.Cities.Add(new City { Index = 624590, Name = "г. Ивдель" });
                    region11.Cities.Add(new City { Index = 623850, Name = "г. Ирбит" });
                    region11.Cities.Add(new City { Index = 623400, Name = "г. Каменск-Уральский" });
                    region11.Cities.Add(new City { Index = 624860, Name = "г. Камышлов" });
                    region11.Cities.Add(new City { Index = 624930, Name = "г. Карпинск" });
                    region11.Cities.Add(new City { Index = 624350, Name = "г. Качканар" });
                    region11.Cities.Add(new City { Index = 624140, Name = "г. Кировград" });
                    region11.Cities.Add(new City { Index = 624440, Name = "г. Краснотурьинск" });
                    region11.Cities.Add(new City { Index = 624330, Name = "г. Красноуральск" });
                    region11.Cities.Add(new City { Index = 623300, Name = "г. Красноуфимск" });
                    region11.Cities.Add(new City { Index = 624300, Name = "г. Кушва" });
                    region11.Cities.Add(new City { Index = 624200, Name = "г. Лесной" });
                    region11.Cities.Add(new City { Index = 623080, Name = "г. Михайловск" });
                    region11.Cities.Add(new City { Index = 624190, Name = "г. Невьянск" });
                    region11.Cities.Add(new City { Index = 623090, Name = "г. Нижние Серги" });
                    region11.Cities.Add(new City { Index = 622000, Name = "г. Нижний Тагил" });
                    region11.Cities.Add(new City { Index = 624740, Name = "г. Нижняя Салда" });
                    region11.Cities.Add(new City { Index = 624220, Name = "г. Нижняя Тура" });
                    region11.Cities.Add(new City { Index = 624400, Name = "г. Новая Ляля" });
                    region11.Cities.Add(new City { Index = 624130, Name = "г. Новоуральск" });
                    region11.Cities.Add(new City { Index = 623100, Name = "г. Первоуральск" });
                    region11.Cities.Add(new City { Index = 623380, Name = "г. Полевской" });
                    region11.Cities.Add(new City { Index = 623280, Name = "г. Ревда" });
                    region11.Cities.Add(new City { Index = 623280, Name = "г. Реж" });
                    region11.Cities.Add(new City { Index = 624480, Name = "г. Североуральск" });
                    region11.Cities.Add(new City { Index = 624000, Name = "г. Серов" });
                    region11.Cities.Add(new City { Index = 624070, Name = "г. Среднеуральск" });
                    region11.Cities.Add(new City { Index = 624800, Name = "г. Сухой Лог" });
                    region11.Cities.Add(new City { Index = 624022, Name = "г. Сысерть" });
                    region11.Cities.Add(new City { Index = 623950, Name = "г. Тавда" });
                    region11.Cities.Add(new City { Index = 623640, Name = "г. Талица" });
                    region11.Cities.Add(new City { Index = 623900, Name = "г. Туринск" });
                    regions.Add(region11);

                var region12 = new RegionCity { Name = "Чувашская республика" };
                    region12.Cities.Add(new City { Index = 429820, Name = "г. Алатырь" });
                    region12.Cities.Add(new City { Index = 429330, Name = "г. Канаш" });
                    region12.Cities.Add(new City { Index = 429430, Name = "г. Козловка" });
                    region12.Cities.Add(new City { Index = 429570, Name = "г. Мариинский Посад" });
                    region12.Cities.Add(new City { Index = 429955, Name = "г. Новочебоксарск" });
                    region12.Cities.Add(new City { Index = 429900, Name = "г. Цивильск" });
                    region12.Cities.Add(new City { Index = 428000, Name = "г. Чебоксары" });
                    region12.Cities.Add(new City { Index = 429120, Name = "г. Шумерля" });
                    region12.Cities.Add(new City { Index = 429060, Name = "г. Ядрин" });
                    regions.Add(region12);

                var region13 = new RegionCity { Name = "Республика Саха (Якутия)" };
                    region13.Cities.Add(new City { Index = 678900, Name = "г. Алдан" });
                    region13.Cities.Add(new City { Index = 678530, Name = "г. Верхоянск" });
                    region13.Cities.Add(new City { Index = 678200, Name = "г. Вилюйск" });
                    region13.Cities.Add(new City { Index = 678140, Name = "г. Ленск" });
                    region13.Cities.Add(new City { Index = 678170, Name = "г. Мирный" });
                    region13.Cities.Add(new City { Index = 678960, Name = "г. Нерюнгри" });
                    region13.Cities.Add(new City { Index = 678450, Name = "г. Нюрба" });
                    region13.Cities.Add(new City { Index = 678100, Name = "г. Олёкминск" });
                    region13.Cities.Add(new City { Index = 678000, Name = "г. Покровск" });
                    region13.Cities.Add(new City { Index = 678790, Name = "г. Среднеколымск" });
                    region13.Cities.Add(new City { Index = 678954, Name = "г. Томмот" });
                    region13.Cities.Add(new City { Index = 678188, Name = "г. Удачный" });
                    region13.Cities.Add(new City { Index = 677000, Name = "г. Якутск" });
                    regions.Add(region13);

                var region14 = new RegionCity { Name = "Камчатский край" };
                    region14.Cities.Add(new City { Index = 684090, Name = "г. Вилючинск" });
                    region14.Cities.Add(new City { Index = 684000, Name = "г. Елизово" });
                    region14.Cities.Add(new City { Index = 683000, Name = "г. Петропавловск-Камчатский" });
                    regions.Add(region14);

                var region15 = new RegionCity { Name = "Алтайский край" };
                    region15.Cities.Add(new City { Index = 658130, Name = "г. Алейск" });
                    region15.Cities.Add(new City { Index = 659300, Name = "г. Бийск" });
                    region15.Cities.Add(new City { Index = 656000, Name = "г. Барнаул" });
                    region15.Cities.Add(new City { Index = 659900, Name = "г. Белокуриха" });
                    region15.Cities.Add(new City { Index = 658420, Name = "г. Горняк" });
                    region15.Cities.Add(new City { Index = 659100, Name = "г. Заринск" });
                    region15.Cities.Add(new City { Index = 658480, Name = "г. Змеиногорск" });
                    region15.Cities.Add(new City { Index = 658700, Name = "г. Камень-на-Оби" });
                    region15.Cities.Add(new City { Index = 658000, Name = "г. Новоалтайск" });
                    region15.Cities.Add(new City { Index = 658200, Name = "г. Рубцовск" });
                    region15.Cities.Add(new City { Index = 658820, Name = "г. Славгород" });
                    region15.Cities.Add(new City { Index = 658837, Name = "г. Яровое" });
                    regions.Add(region15);

                var region16 = new RegionCity { Name = "Владимирская область" };
                    region16.Cities.Add(new City { Index = 601650, Name = "г. Александров" });
                    region16.Cities.Add(new City { Index = 600000, Name = "г. Владимир" });
                    region16.Cities.Add(new City { Index = 601440, Name = "г. Вязники" });
                    region16.Cities.Add(new City { Index = 601480, Name = "г. Гороховец" });
                    region16.Cities.Add(new City { Index = 601501, Name = "г. Гусь-Хрустальный" });
                    region16.Cities.Add(new City { Index = 601300, Name = "г. Камешково" });
                    region16.Cities.Add(new City { Index = 601640, Name = "г. Карабаново" });
                    region16.Cities.Add(new City { Index = 601010, Name = "г. Киржач" });
                    region16.Cities.Add(new City { Index = 601900, Name = "г. Ковров" });
                    region16.Cities.Add(new City { Index = 601780, Name = "г. Кольчугино" });
                    region16.Cities.Add(new City { Index = 601110, Name = "г. Костерево" });
                    region16.Cities.Add(new City { Index = 601570, Name = "г. Курлово" });
                    region16.Cities.Add(new City { Index = 601240, Name = "г. Лакинск" });
                    region16.Cities.Add(new City { Index = 602102, Name = "г. Меленки" });
                    region16.Cities.Add(new City { Index = 602200, Name = "г. Муром" });
                    region16.Cities.Add(new City { Index = 601144, Name = "г. Петушки" });
                    region16.Cities.Add(new City { Index = 601120, Name = "г. Покров" });
                    region16.Cities.Add(new City { Index = 600910, Name = "г. Радужный" });
                    region16.Cities.Add(new City { Index = 601200, Name = "г. Собинка" });
                    region16.Cities.Add(new City { Index = 601670, Name = "г. Струнино" });
                    region16.Cities.Add(new City { Index = 601350, Name = "г. Судогда" });
                    region16.Cities.Add(new City { Index = 601293, Name = "г. Суздаль" });
                    region16.Cities.Add(new City { Index = 601800, Name = "г. Юрьев-Польский" });
                    regions.Add(region16);

                var region17 = new RegionCity { Name = "Пермский край" };
                    region17.Cities.Add(new City { Index = 618320, Name = "г. Александровск" });
                    region17.Cities.Add(new City { Index = 618400, Name = "г. Березники" });
                    region17.Cities.Add(new City { Index = 617120, Name = "г. Верещагино" });
                    region17.Cities.Add(new City { Index = 618820, Name = "г. Горнозаводск" });
                    region17.Cities.Add(new City { Index = 618270, Name = "г. Гремячинск" });
                    region17.Cities.Add(new City { Index = 618250, Name = "г. Губаха" });
                    region17.Cities.Add(new City { Index = 618740, Name = "г. Добрянка" });
                    region17.Cities.Add(new City { Index = 618350, Name = "г. Кизел" });
                    region17.Cities.Add(new City { Index = 618590, Name = "г. Красновишерск" });
                    region17.Cities.Add(new City { Index = 617060, Name = "г. Краснокамск" });
                    region17.Cities.Add(new City { Index = 619000, Name = "г. Кудымкар" });
                    region17.Cities.Add(new City { Index = 617470, Name = "г. Кунгур" });
                    region17.Cities.Add(new City { Index = 618900, Name = "г. Лысьва" });
                    region17.Cities.Add(new City { Index = 617000, Name = "г. Нытва" });
                    region17.Cities.Add(new City { Index = 618120, Name = "г. Оса" });
                    region17.Cities.Add(new City { Index = 618100, Name = "г. Оханск" });
                    region17.Cities.Add(new City { Index = 617140, Name = "г. Очёр" });
                    region17.Cities.Add(new City { Index = 614000, Name = "г. Пермь" });
                    region17.Cities.Add(new City { Index = 618540, Name = "г. Соликамск" });
                    region17.Cities.Add(new City { Index = 618460, Name = "г. Усолье" });
                    region17.Cities.Add(new City { Index = 617760, Name = "г. Чайковский" });
                    region17.Cities.Add(new City { Index = 618600, Name = "г. Чердынь" });
                    region17.Cities.Add(new City { Index = 617040, Name = "г. Чёрмоз" });
                    region17.Cities.Add(new City { Index = 617830, Name = "г. Чернушка" });
                    region17.Cities.Add(new City { Index = 618204, Name = "г. Чусовой" });
                    regions.Add(region17);

                var region18 = new RegionCity { Name = "Сахалинская область" };
                    region18.Cities.Add(new City { Index = 618320, Name = "г. Александровск-Сахалинский" });
                    region18.Cities.Add(new City { Index = 694030, Name = "г. Анива" });
                    region18.Cities.Add(new City { Index = 694760, Name = "г. Горнозаводск" });
                    region18.Cities.Add(new City { Index = 694051, Name = "г. Долинск" });
                    region18.Cities.Add(new City { Index = 694020, Name = "г. Корсаков" });
                    region18.Cities.Add(new City { Index = 694530, Name = "г. Курильск" });
                    region18.Cities.Add(new City { Index = 694140, Name = "г. Макаров" });
                    region18.Cities.Add(new City { Index = 694740, Name = "г. Невельск" });
                    region18.Cities.Add(new City { Index = 694490, Name = "г. Оха" });
                    region18.Cities.Add(new City { Index = 694240, Name = "г. Поронайск" });
                    region18.Cities.Add(new City { Index = 694550, Name = "г. Северо-Курильск" });
                    region18.Cities.Add(new City { Index = 694820, Name = "г. Томари" });
                    region18.Cities.Add(new City { Index = 694920, Name = "г. Углегорск" });
                    region18.Cities.Add(new City { Index = 694620, Name = "г. Холмск" });
                    region18.Cities.Add(new City { Index = 694910, Name = "г. Шахтёрск" });
                    region18.Cities.Add(new City { Index = 693000, Name = "г. Южно-Сахалинск" });
                    regions.Add(region18);

                var region19 = new RegionCity { Name = "Белгородская область" };
                    region19.Cities.Add(new City { Index = 618320, Name = "г. Алексеевка" });
                    region19.Cities.Add(new City { Index = 308000, Name = "г. Белгород" });
                    region19.Cities.Add(new City { Index = 309920, Name = "г. Бирюч" });
                    region19.Cities.Add(new City { Index = 309990, Name = "г. Валуйки" });
                    region19.Cities.Add(new City { Index = 309372, Name = "г. Грайворон" });
                    region19.Cities.Add(new City { Index = 309180, Name = "г. Губкин" });
                    region19.Cities.Add(new City { Index = 309210, Name = "г. Короча" });
                    region19.Cities.Add(new City { Index = 309640, Name = "г. Новый Оскол" });
                    region19.Cities.Add(new City { Index = 309500, Name = "г. Старый Оскол" });
                    region19.Cities.Add(new City { Index = 309070, Name = "г. Строитель" });
                    region19.Cities.Add(new City { Index = 309290, Name = "г. Шебекино" });
                    regions.Add(region19);

                var region20 = new RegionCity { Name = "Тульская область" };
                    region20.Cities.Add(new City { Index = 618320, Name = "г. Алексин" });
                    region20.Cities.Add(new City { Index = 301530, Name = "г. Белёв" });
                    region20.Cities.Add(new City { Index = 301835, Name = "г. Богородицк" });
                    region20.Cities.Add(new City { Index = 301280, Name = "г. Болохово" });
                    region20.Cities.Add(new City { Index = 301320, Name = "г. Венёв" });
                    region20.Cities.Add(new City { Index = 301760, Name = "г. Донской" });
                    region20.Cities.Add(new City { Index = 301840, Name = "г. Ефремов" });
                    region20.Cities.Add(new City { Index = 301720, Name = "г. Кимовск" });
                    region20.Cities.Add(new City { Index = 301260, Name = "г. Киреевск" });
                    region20.Cities.Add(new City { Index = 301264, Name = "г. Липки" });
                    region20.Cities.Add(new City { Index = 301650, Name = "г. Новомосковск" });
                    region20.Cities.Add(new City { Index = 301470, Name = "г. Плавск" });
                    region20.Cities.Add(new City { Index = 301205, Name = "г. Советск" });
                    region20.Cities.Add(new City { Index = 301430, Name = "г. Суворов" });
                    region20.Cities.Add(new City { Index = 300000, Name = "г. Тула" });
                    region20.Cities.Add(new City { Index = 301602, Name = "г. Узловая" });
                    region20.Cities.Add(new City { Index = 301414, Name = "г. Чекалин" });
                    region20.Cities.Add(new City { Index = 301240, Name = "г. Щёкино" });
                    region20.Cities.Add(new City { Index = 301030, Name = "г. Ясногорск" });
                    regions.Add(region20);

                var region21 = new RegionCity { Name = "Иркутская область" };
                    region21.Cities.Add(new City { Index = 665117, Name = "г. Алзамай" });
                    region21.Cities.Add(new City { Index = 665800, Name = "г. Ангарск" });
                    region21.Cities.Add(new City { Index = 665930, Name = "г. Байкальск" });
                    region21.Cities.Add(new City { Index = 665050, Name = "г. Бирюсинск" });
                    region21.Cities.Add(new City { Index = 666904, Name = "г. Бодайбо" });
                    region21.Cities.Add(new City { Index = 665700, Name = "г. Братск" });
                    region21.Cities.Add(new City { Index = 665770, Name = "г. Вихоревка" });
                    region21.Cities.Add(new City { Index = 665653, Name = "г. Железногорск-Илимский" });
                    region21.Cities.Add(new City { Index = 665393, Name = "г. Зима" });
                    region21.Cities.Add(new City { Index = 664000, Name = "г. Иркутск" });
                    region21.Cities.Add(new City { Index = 666703, Name = "г. Киренск" });
                    region21.Cities.Add(new City { Index = 665106, Name = "г. Нижнеудинск" });
                    region21.Cities.Add(new City { Index = 666300, Name = "г. Саянск" });
                    region21.Cities.Add(new City { Index = 665420, Name = "г. Свирск" });
                    region21.Cities.Add(new City { Index = 665900, Name = "г. Слюдянка" });
                    region21.Cities.Add(new City { Index = 665009, Name = "г. Тайшет" });
                    region21.Cities.Add(new City { Index = 665220, Name = "г. Тулун" });
                    region21.Cities.Add(new City { Index = 665460, Name = "г. Усолье-Сибирское" });
                    region21.Cities.Add(new City { Index = 666600, Name = "г. Усть-Илимск" });
                    region21.Cities.Add(new City { Index = 666780, Name = "г. Усть-Кут" });
                    region21.Cities.Add(new City { Index = 665400, Name = "г. Черемхово" });
                    region21.Cities.Add(new City { Index = 666030, Name = "г. Шелехов" });
                    regions.Add(region21);

                var region22 = new RegionCity { Name = "Хабаровский край" };
                    region22.Cities.Add(new City { Index = 682643, Name = "г. Амурск" });
                    region22.Cities.Add(new City { Index = 682970, Name = "г. Бикин" });
                    region22.Cities.Add(new City { Index = 682950, Name = "г. Вяземский" });
                    region22.Cities.Add(new City { Index = 681000, Name = "г. Комсомольск-на-Амуре" });
                    region22.Cities.Add(new City { Index = 682460, Name = "г. Николаевск-на-Амуре" });
                    region22.Cities.Add(new City { Index = 682800, Name = "г. Советская Гавань" });
                    region22.Cities.Add(new City { Index = 680000, Name = "г. Хабаровск" });
                    regions.Add(region22);

                var region23 = new RegionCity { Name = "Чукотский автономный округ" };
                    region23.Cities.Add(new City { Index = 689000, Name = "г. Анадырь" });
                    region23.Cities.Add(new City { Index = 689450, Name = "г. Билибино" });
                    region23.Cities.Add(new City { Index = 689400, Name = "г. Певек" });
                    regions.Add(region23);

                var region24 = new RegionCity { Name = "Приморский край" };
                    region24.Cities.Add(new City { Index = 692330, Name = "г. Арсеньев" });
                    region24.Cities.Add(new City { Index = 692760, Name = "г. Артём" });
                    region24.Cities.Add(new City { Index = 692800, Name = "г. Большой Камень" });
                    region24.Cities.Add(new City { Index = 690000, Name = "г. Владивосток" });
                    region24.Cities.Add(new City { Index = 692441, Name = "г. Дальнегорск" });
                    region24.Cities.Add(new City { Index = 692030, Name = "г. Лесозаводск" });
                    region24.Cities.Add(new City { Index = 692900, Name = "г. Находка" });
                    region24.Cities.Add(new City { Index = 692864, Name = "г. Партизанск" });
                    region24.Cities.Add(new City { Index = 692245, Name = "г. Спасск-Дальний" });
                    region24.Cities.Add(new City { Index = 692500, Name = "г. Уссурийск" });
                    region24.Cities.Add(new City { Index = 692880, Name = "г. Фокино" });
                    regions.Add(region24);

                var region25 = new RegionCity { Name = "Тверская область" };
                    region25.Cities.Add(new City { Index = 172800, Name = "г. Андреаполь" });
                    region25.Cities.Add(new City { Index = 171980, Name = "г. Бежецк" });
                    region25.Cities.Add(new City { Index = 172530, Name = "г. Белый" });
                    region25.Cities.Add(new City { Index = 171070, Name = "г. Бологое" });
                    region25.Cities.Add(new City { Index = 171720, Name = "г. Весьегонск" });
                    region25.Cities.Add(new City { Index = 171151, Name = "г. Вышний Волочёк" });
                    region25.Cities.Add(new City { Index = 172610, Name = "г. Западная Двина" });
                    region25.Cities.Add(new City { Index = 172333, Name = "г. Зубцов" });
                    region25.Cities.Add(new City { Index = 171570, Name = "г. Калязин" });
                    region25.Cities.Add(new City { Index = 171640, Name = "г. Кашин" });
                    region25.Cities.Add(new City { Index = 171500, Name = "г. Кимры" });
                    region25.Cities.Add(new City { Index = 171252, Name = "г. Конаково" });
                    region25.Cities.Add(new City { Index = 171660, Name = "г. Красный Холм" });
                    region25.Cities.Add(new City { Index = 172110, Name = "г. Кувшиново" });
                    region25.Cities.Add(new City { Index = 171210, Name = "г. Лихославль" });
                    region25.Cities.Add(new City { Index = 172520, Name = "г. Нелидово" });
                    region25.Cities.Add(new City { Index = 172730, Name = "г. Осташков" });
                    region25.Cities.Add(new City { Index = 172380, Name = "г. Ржев" });
                    region25.Cities.Add(new City { Index = 171360, Name = "г. Старица" });
                    region25.Cities.Add(new City { Index = 170000, Name = "г. Тверь" });
                    region25.Cities.Add(new City { Index = 172000, Name = "г. Торжок" });
                    region25.Cities.Add(new City { Index = 172840, Name = "г. Торопец" });
                    region25.Cities.Add(new City { Index = 171840, Name = "г. Удомля" });
                    regions.Add(region25);

                var region26 = new RegionCity { Name = "Кемеровская область" };
                    region26.Cities.Add(new City { Index = 652470, Name = "г. Анжеро-Судженск" });
                    region26.Cities.Add(new City { Index = 652600, Name = "г. Белово" });
                    region26.Cities.Add(new City { Index = 652780, Name = "г. Гурьевск" });
                    region26.Cities.Add(new City { Index = 652740, Name = "г. Калтан" });
                    region26.Cities.Add(new City { Index = 650000, Name = "г. Кемерово" });
                    region26.Cities.Add(new City { Index = 652700, Name = "г. Киселёвск" });
                    region26.Cities.Add(new City { Index = 652500, Name = "г. Ленинск-Кузнецкий" });
                    region26.Cities.Add(new City { Index = 652150, Name = "г. Мариинск" });
                    region26.Cities.Add(new City { Index = 652870, Name = "г. Междуреченск" });
                    region26.Cities.Add(new City { Index = 652840, Name = "г. Мыски" });
                    region26.Cities.Add(new City { Index = 654000, Name = "г. Новокузнецк" });
                    region26.Cities.Add(new City { Index = 652800, Name = "г. Осинники" });
                    region26.Cities.Add(new City { Index = 652560, Name = "г. Полысаево" });
                    region26.Cities.Add(new City { Index = 653000, Name = "г. Прокопьевск" });
                    region26.Cities.Add(new City { Index = 652770, Name = "г. Салаир" });
                    region26.Cities.Add(new City { Index = 652400, Name = "г. Тайга" });
                    region26.Cities.Add(new City { Index = 652990, Name = "г. Таштагол" });
                    region26.Cities.Add(new City { Index = 652300, Name = "г. Топки" });
                    region26.Cities.Add(new City { Index = 652050, Name = "г. Юрга" });
                    regions.Add(region26);

                var region27 = new RegionCity { Name = "Мурманская область" };
                    region27.Cities.Add(new City { Index = 184209, Name = "г. Апатиты" });
                    region27.Cities.Add(new City { Index = 184670, Name = "г. Гаджиево" });
                    region27.Cities.Add(new City { Index = 184310, Name = "г. Заозёрск" });
                    region27.Cities.Add(new City { Index = 184430, Name = "г. Заполярный" });
                    region27.Cities.Add(new City { Index = 184041, Name = "г. Кандалакша" });
                    region27.Cities.Add(new City { Index = 184250, Name = "г. Кировск" });
                    region27.Cities.Add(new City { Index = 184144, Name = "г. Ковдор" });
                    region27.Cities.Add(new City { Index = 184380, Name = "г. Кола" });
                    region27.Cities.Add(new City { Index = 184505, Name = "г. Мончегорск" });
                    region27.Cities.Add(new City { Index = 183038, Name = "г. Мурманск" });
                    region27.Cities.Add(new City { Index = 184530, Name = "г. Оленегорск" });
                    region27.Cities.Add(new City { Index = 184640, Name = "г. Островной" });
                    region27.Cities.Add(new City { Index = 184230, Name = "г. Полярные Зори" });
                    region27.Cities.Add(new City { Index = 184650, Name = "г. Полярный" });
                    region27.Cities.Add(new City { Index = 184606, Name = "г. Североморск" });
                    region27.Cities.Add(new City { Index = 184682, Name = "г. Снежногорск" });
                    regions.Add(region27);

                var region28 = new RegionCity { Name = "Московская область" };
                    region28.Cities.Add(new City { Index = 143360, Name = "г. Апрелевка" });
                    region28.Cities.Add(new City { Index = 143900, Name = "г. Балашиха" });
                    region28.Cities.Add(new City { Index = 140170, Name = "г. Бронницы" });
                    region28.Cities.Add(new City { Index = 143330, Name = "г. Верея" });
                    region28.Cities.Add(new City { Index = 142700, Name = "г. Видное" });
                    region28.Cities.Add(new City { Index = 143600, Name = "г. Волоколамск" });
                    region28.Cities.Add(new City { Index = 140200, Name = "г. Воскресенск" });
                    region28.Cities.Add(new City { Index = 141650, Name = "г. Высоковск" });
                    region28.Cities.Add(new City { Index = 143530, Name = "г. Дедовск" });
                    region28.Cities.Add(new City { Index = 140090, Name = "г. Дзержинский" });
                    region28.Cities.Add(new City { Index = 141800, Name = "г. Дмитров" });
                    region28.Cities.Add(new City { Index = 141707, Name = "г. Долгопрудный" });
                    region28.Cities.Add(new City { Index = 142000, Name = "г. Домодедово" });
                    region28.Cities.Add(new City { Index = 142660, Name = "г. Дрезна" });
                    region28.Cities.Add(new City { Index = 141980, Name = "г. Дубна" });
                    region28.Cities.Add(new City { Index = 140300, Name = "г. Егорьевск" });
                    region28.Cities.Add(new City { Index = 140180, Name = "г. Жуковский" });
                    region28.Cities.Add(new City { Index = 140600, Name = "г. Зарайск" });
                    region28.Cities.Add(new City { Index = 143180, Name = "г. Звенигород" });
                    region28.Cities.Add(new City { Index = 141280, Name = "г. Ивантеевка" });
                    region28.Cities.Add(new City { Index = 143500, Name = "г. Истра" });
                    region28.Cities.Add(new City { Index = 142900, Name = "г. Кашира" });
                    region28.Cities.Add(new City { Index = 142180, Name = "г. Климовск" });
                    region28.Cities.Add(new City { Index = 141600, Name = "г. Клин" });
                    region28.Cities.Add(new City { Index = 141656, Name = "г. Княгинино" });
                    region28.Cities.Add(new City { Index = 140400, Name = "г. Коломна" });
                    region28.Cities.Add(new City { Index = 141068, Name = "г. Королёв" });
                    region28.Cities.Add(new City { Index = 141290, Name = "г. Красноармейск" });
                    region28.Cities.Add(new City { Index = 143402, Name = "г. Красногорск" });
                    region28.Cities.Add(new City { Index = 141321, Name = "г. Краснозаводск" });
                    region28.Cities.Add(new City { Index = 143090, Name = "г. Краснознаменск" });
                    region28.Cities.Add(new City { Index = 142621, Name = "г. Куровское" });
                    region28.Cities.Add(new City { Index = 142670, Name = "г. Ликино-Дулёво" });
                    region28.Cities.Add(new City { Index = 141730, Name = "г. Лобня" });
                    region28.Cities.Add(new City { Index = 141150, Name = "г. Лосино-Петровский" });
                    region28.Cities.Add(new City { Index = 140501, Name = "г. Луховицы" });
                    region28.Cities.Add(new City { Index = 140080, Name = "г. Лыткарино" });
                    region28.Cities.Add(new City { Index = 140000, Name = "г. Люберцы" });
                    region28.Cities.Add(new City { Index = 143200, Name = "г. Можайск" });
                    region28.Cities.Add(new City { Index = 101000, Name = "г. Москва" });
                    region28.Cities.Add(new City { Index = 141000, Name = "г. Мытищи" });
                    region28.Cities.Add(new City { Index = 143301, Name = "г. Наро-Фоминск" });
                    region28.Cities.Add(new City { Index = 142400, Name = "г. Ногинск" });
                    region28.Cities.Add(new City { Index = 143000, Name = "г. Одинцово" });
                    region28.Cities.Add(new City { Index = 140560, Name = "г. Озёры" });
                    region28.Cities.Add(new City { Index = 142600, Name = "г. Орехово-Зуево" });
                    region28.Cities.Add(new City { Index = 142500, Name = "г. Павловский Посад" });
                    region28.Cities.Add(new City { Index = 141320, Name = "г. Пересвет" });
                    region28.Cities.Add(new City { Index = 142100, Name = "г. Подольск" });
                    region28.Cities.Add(new City { Index = 142280, Name = "г. Протвино" });
                    region28.Cities.Add(new City { Index = 141200, Name = "г. Пушкино" });
                    region28.Cities.Add(new City { Index = 142290, Name = "г. Пущино" });
                    region28.Cities.Add(new City { Index = 140100, Name = "г. Раменское" });
                    region28.Cities.Add(new City { Index = 143960, Name = "г. Реутов" });
                    region28.Cities.Add(new City { Index = 140730, Name = "г. Рошаль" });
                    region28.Cities.Add(new City { Index = 143103, Name = "г. Руза" });
                    region28.Cities.Add(new City { Index = 141300, Name = "г. Сергиев Посад" });
                    region28.Cities.Add(new City { Index = 142200, Name = "г. Серпухов" });
                    region28.Cities.Add(new City { Index = 141501, Name = "г. Солнечногорск" });
                    region28.Cities.Add(new City { Index = 142800, Name = "г. Ступино" });
                    region28.Cities.Add(new City { Index = 141900, Name = "г. Талдом" });
                    region28.Cities.Add(new City { Index = 108840, Name = "г. Троицк" });
                    region28.Cities.Add(new City { Index = 141190, Name = "г. Фрязино" });
                    region28.Cities.Add(new City { Index = 141400, Name = "г. Химки" });
                    region28.Cities.Add(new City { Index = 141371, Name = "г. Хотьково" });
                    region28.Cities.Add(new City { Index = 142432, Name = "г. Черноголовка" });
                    region28.Cities.Add(new City { Index = 142300, Name = "г. Чехов" });
                    region28.Cities.Add(new City { Index = 140700, Name = "г. Шатура" });
                    region28.Cities.Add(new City { Index = 141100, Name = "г. Щёлково" });
                    region28.Cities.Add(new City { Index = 142530, Name = "г. Электрогорск" });
                    region28.Cities.Add(new City { Index = 144000, Name = "г. Электросталь" });
                    region28.Cities.Add(new City { Index = 142455, Name = "г. Электроугли" });
                    region28.Cities.Add(new City { Index = 141840, Name = "г. Яхрома" });
                    regions.Add(region28);

                var region29 = new RegionCity { Name = "Чеченская республика" };
                    region29.Cities.Add(new City { Index = 366281, Name = "г. Аргун" });
                    region29.Cities.Add(new City { Index = 364000, Name = "г. Грозный" });
                    region29.Cities.Add(new City { Index = 366200, Name = "г. Гудермес" });
                    region29.Cities.Add(new City { Index = 366500, Name = "г. Урус-Мартан" });
                    region29.Cities.Add(new City { Index = 366300, Name = "г. Шали" });
                    regions.Add(region29);

                var region30 = new RegionCity { Name = "Республика Мордовия" };
                    region30.Cities.Add(new City { Index = 431860, Name = "г. Ардатов" });
                    region30.Cities.Add(new City { Index = 431430, Name = "г. Инсар" });
                    region30.Cities.Add(new City { Index = 431350, Name = "г. Ковылкино" });
                    region30.Cities.Add(new City { Index = 431260, Name = "г. Краснослободск" });
                    region30.Cities.Add(new City { Index = 431440, Name = "г. Рузаевка" });
                    region30.Cities.Add(new City { Index = 430000, Name = "г. Саранск" });
                    region30.Cities.Add(new City { Index = 431220, Name = "г. Темников" });
                    regions.Add(region30);

                var region31 = new RegionCity { Name = "Нижегородская область" };
                    region31.Cities.Add(new City { Index = 607220, Name = "г. Арзамас" });
                    region31.Cities.Add(new City { Index = 606400, Name = "г. Балахна" });
                    region31.Cities.Add(new City { Index = 607600, Name = "г. Богородск" });
                    region31.Cities.Add(new City { Index = 606440, Name = "г. Бор" });
                    region31.Cities.Add(new City { Index = 606860, Name = "г. Ветлуга" });
                    region31.Cities.Add(new City { Index = 606072, Name = "г. Володарск" });
                    region31.Cities.Add(new City { Index = 606120, Name = "г. Ворсма" });
                    region31.Cities.Add(new City { Index = 607060, Name = "г. Выкса" });
                    region31.Cities.Add(new City { Index = 606125, Name = "г. Горбатов" });
                    region31.Cities.Add(new City { Index = 606500, Name = "г. Городец" });
                    region31.Cities.Add(new City { Index = 606000, Name = "г. Дзержинск" });
                    region31.Cities.Add(new City { Index = 606520, Name = "г. Заволжье" });
                    region31.Cities.Add(new City { Index = 606340, Name = "г. Княгинино" });
                    region31.Cities.Add(new City { Index = 607650, Name = "г. Кстово" });
                    region31.Cities.Add(new City { Index = 607010, Name = "г. Кулебаки" });
                    region31.Cities.Add(new City { Index = 607800, Name = "г. Лукоянов" });
                    region31.Cities.Add(new City { Index = 606210, Name = "г. Лысково" });
                    region31.Cities.Add(new City { Index = 607100, Name = "г. Навашино" });
                    region31.Cities.Add(new City { Index = 603000, Name = "г. Нижний Новгород" });
                    region31.Cities.Add(new City { Index = 606100, Name = "г. Павлово" });
                    region31.Cities.Add(new City { Index = 607760, Name = "г. Первомайск" });
                    region31.Cities.Add(new City { Index = 607400, Name = "г. Перевоз" });
                    region31.Cities.Add(new City { Index = 607181, Name = "г. Саров" });
                    region31.Cities.Add(new City { Index = 606650, Name = "г. Семёнов" });
                    region31.Cities.Add(new City { Index = 607510, Name = "г. Сергач" });
                    region31.Cities.Add(new City { Index = 606800, Name = "г. Урень" });
                    region31.Cities.Add(new City { Index = 606540, Name = "г. Чкаловск" });
                    region31.Cities.Add(new City { Index = 606910, Name = "г. Шахунья" });
                    regions.Add(region31);

                var region32 = new RegionCity { Name = "Саратовская область" };
                    region32.Cities.Add(new City { Index = 412210, Name = "г. Аркадак" });
                    region32.Cities.Add(new City { Index = 412420, Name = "г. Аткарск" });
                    region32.Cities.Add(new City { Index = 413840, Name = "г. Балаково" });
                    region32.Cities.Add(new City { Index = 412302, Name = "г. Балашов" });
                    region32.Cities.Add(new City { Index = 412900, Name = "г. Вольск" });
                    region32.Cities.Add(new City { Index = 413500, Name = "г. Ершов" });
                    region32.Cities.Add(new City { Index = 412480, Name = "г. Калининск" });
                    region32.Cities.Add(new City { Index = 412800, Name = "г. Красноармейск" });
                    region32.Cities.Add(new City { Index = 413231, Name = "г. Красный Кут" });
                    region32.Cities.Add(new City { Index = 413090, Name = "г. Маркс" });
                    region32.Cities.Add(new City { Index = 413360, Name = "г. Новоузенск" });
                    region32.Cities.Add(new City { Index = 412540, Name = "г. Петровск" });
                    region32.Cities.Add(new City { Index = 413720, Name = "г. Пугачёв" });
                    region32.Cities.Add(new City { Index = 412030, Name = "г. Ртищево" });
                    region32.Cities.Add(new City { Index = 410000, Name = "г. Саратов" });
                    region32.Cities.Add(new City { Index = 412780, Name = "г. Хвалынск" });
                    region32.Cities.Add(new City { Index = 412950, Name = "г. Шиханы" });
                    region32.Cities.Add(new City { Index = 413100, Name = "г. Энгельс" });
                    regions.Add(region32);

                var region33 = new RegionCity { Name = "Красноярский край" };
                    region33.Cities.Add(new City { Index = 662951, Name = "г. Артёмовск" });
                    region33.Cities.Add(new City { Index = 662150, Name = "г. Ачинск" });
                    region33.Cities.Add(new City { Index = 662060, Name = "г. Боготол" });
                    region33.Cities.Add(new City { Index = 663980, Name = "г. Бородино" });
                    region33.Cities.Add(new City { Index = 663090, Name = "г. Дивногорск" });
                    region33.Cities.Add(new City { Index = 647000, Name = "г. Дудинка" });
                    region33.Cities.Add(new City { Index = 663180, Name = "г. Енисейск" });
                    region33.Cities.Add(new City { Index = 662970, Name = "г. Железногорск" });
                    region33.Cities.Add(new City { Index = 663960, Name = "г. Заозёрный" });
                    region33.Cities.Add(new City { Index = 663690, Name = "г. Зеленогорск" });
                    region33.Cities.Add(new City { Index = 663200, Name = "г. Игарка" });
                    region33.Cities.Add(new City { Index = 663800, Name = "г. Иланский" });
                    region33.Cities.Add(new City { Index = 663600, Name = "г. Канск" });
                    region33.Cities.Add(new City { Index = 663491, Name = "г. Кодинск" });
                    region33.Cities.Add(new City { Index = 660000, Name = "г. Красноярск" });
                    region33.Cities.Add(new City { Index = 662541, Name = "г. Лесосибирск" });
                    region33.Cities.Add(new City { Index = 662600, Name = "г. Минусинск" });
                    region33.Cities.Add(new City { Index = 662200, Name = "г. Назарово" });
                    region33.Cities.Add(new City { Index = 663300, Name = "г. Норильск" });
                    region33.Cities.Add(new City { Index = 663335, Name = "п.г.т. Снежногорск" });
                    region33.Cities.Add(new City { Index = 662500, Name = "г. Сосновоборск" });
                    region33.Cities.Add(new City { Index = 662251, Name = "г. Ужур" });
                    region33.Cities.Add(new City { Index = 663920, Name = "г. Уяр" });
                    region33.Cities.Add(new City { Index = 662305, Name = "г. Шарыпово" });
                    regions.Add(region33);

                var region34 = new RegionCity { Name = "Архангельская область" };
                    region34.Cities.Add(new City { Index = 163000, Name = "г. Архангельск" });
                    region34.Cities.Add(new City { Index = 165150, Name = "г. Вельск" });
                    region34.Cities.Add(new City { Index = 164110, Name = "г. Каргополь" });
                    region34.Cities.Add(new City { Index = 165650, Name = "г. Коряжма" });
                    region34.Cities.Add(new City { Index = 165300, Name = "г. Котлас" });
                    region34.Cities.Add(new City { Index = 164750, Name = "г. Мезень" });
                    region34.Cities.Add(new City { Index = 164170, Name = "г. Мирный" });
                    region34.Cities.Add(new City { Index = 164900, Name = "г. Новодвинск" });
                    region34.Cities.Add(new City { Index = 164200, Name = "г. Няндома" });
                    region34.Cities.Add(new City { Index = 164840, Name = "г. Онега" });
                    region34.Cities.Add(new City { Index = 164500, Name = "г. Северодвинск" });
                    region34.Cities.Add(new City { Index = 165330, Name = "г. Сольвычегодск" });
                    region34.Cities.Add(new City { Index = 165160, Name = "г. Шенкурск" });
                    regions.Add(region34);

                var region35 = new RegionCity { Name = "Томская область" };
                    region35.Cities.Add(new City { Index = 636840, Name = "г. Асино" });
                    region35.Cities.Add(new City { Index = 636615, Name = "г. Кедровый" });
                    region35.Cities.Add(new City { Index = 636460, Name = "г. Колпашево" });
                    region35.Cities.Add(new City { Index = 636000, Name = "г. Северск" });
                    region35.Cities.Add(new City { Index = 636780, Name = "г. Стрежевой" });
                    region35.Cities.Add(new City { Index = 634000, Name = "г. Томск" });
                    regions.Add(region35);

                var region36 = new RegionCity { Name = "Астраханская область" };
                    region36.Cities.Add(new City { Index = 414000, Name = "г. Астрахань" });
                    region36.Cities.Add(new City { Index = 416500, Name = "г. Ахтубинск" });
                    region36.Cities.Add(new City { Index = 416540, Name = "г. Знаменск" });
                    region36.Cities.Add(new City { Index = 416340, Name = "г. Камызяк" });
                    region36.Cities.Add(new City { Index = 416111, Name = "г. Нариманов" });
                    region36.Cities.Add(new City { Index = 416010, Name = "г. Харабали" });
                    regions.Add(region36);

                var region37 = new RegionCity { Name = "Челябинская область" };
                    region37.Cities.Add(new City { Index = 456010, Name = "г. Аша" });
                    region37.Cities.Add(new City { Index = 456900, Name = "г. Бакал" });
                    region37.Cities.Add(new City { Index = 457670, Name = "г. Верхнеуральск" });
                    region37.Cities.Add(new City { Index = 456800, Name = "г. Верхний Уфалей" });
                    region37.Cities.Add(new City { Index = 456580, Name = "г. Еманжелинск" });
                    region37.Cities.Add(new City { Index = 456200, Name = "г. Златоуст" });
                    region37.Cities.Add(new City { Index = 456140, Name = "г. Карабаш" });
                    region37.Cities.Add(new City { Index = 457359, Name = "г. Карталы" });
                    region37.Cities.Add(new City { Index = 456830, Name = "г. Касли" });
                    region37.Cities.Add(new City { Index = 456110, Name = "г. Катав-Ивановск" });
                    region37.Cities.Add(new City { Index = 456600, Name = "г. Копейск" });
                    region37.Cities.Add(new City { Index = 456550, Name = "г. Коркино" });
                    region37.Cities.Add(new City { Index = 456940, Name = "г. Куса" });
                    region37.Cities.Add(new City { Index = 456870, Name = "г. Кыштым" });
                    region37.Cities.Add(new City { Index = 455000, Name = "г. Магнитогорск" });
                    region37.Cities.Add(new City { Index = 456300, Name = "г. Миасс" });
                    region37.Cities.Add(new City { Index = 456007, Name = "г. Миньяр" });
                    region37.Cities.Add(new City { Index = 456970, Name = "г. Нязепетровск" });
                    region37.Cities.Add(new City { Index = 456780, Name = "г. Озёрск" });
                    region37.Cities.Add(new City { Index = 457020, Name = "г. Пласт" });
                    region37.Cities.Add(new City { Index = 456913, Name = "г. Сатка" });
                    region37.Cities.Add(new City { Index = 456022, Name = "г. Сим" });
                    region37.Cities.Add(new City { Index = 456770, Name = "г. Снежинск" });
                    region37.Cities.Add(new City { Index = 456080, Name = "г. Трехгорный" });
                    region37.Cities.Add(new City { Index = 457100, Name = "г. Троицк" });
                    region37.Cities.Add(new City { Index = 456040, Name = "г. Усть-Катав" });
                    region37.Cities.Add(new City { Index = 456440, Name = "г. Чебаркуль" });
                    region37.Cities.Add(new City { Index = 454000, Name = "г. Челябинск" });
                    region37.Cities.Add(new City { Index = 457040, Name = "г. Южноуральск" });
                    region37.Cities.Add(new City { Index = 456120, Name = "г. Юрюзань" });
                    regions.Add(region37);

                var region38 = new RegionCity { Name = "Вологодская область" };
                    region38.Cities.Add(new City { Index = 162480, Name = "г. Бабаево" });
                    region38.Cities.Add(new City { Index = 161200, Name = "г. Белозерск" });
                    region38.Cities.Add(new City { Index = 162390, Name = "г. Великий Устюг" });
                    region38.Cities.Add(new City { Index = 160000, Name = "г. Вологда" });
                    region38.Cities.Add(new City { Index = 162900, Name = "г. Вытегра" });
                    region38.Cities.Add(new City { Index = 162000, Name = "г. Грязовец" });
                    region38.Cities.Add(new City { Index = 162107, Name = "г. Кадников" });
                    region38.Cities.Add(new City { Index = 161100, Name = "г. Кириллов" });
                    region38.Cities.Add(new City { Index = 162341, Name = "г. Красавино" });
                    region38.Cities.Add(new City { Index = 162129, Name = "г. Сокол" });
                    region38.Cities.Add(new City { Index = 161300, Name = "г. Тотьма" });
                    region38.Cities.Add(new City { Index = 162840, Name = "г. Устюжна" });
                    region38.Cities.Add(new City { Index = 162250, Name = "г. Харовск" });
                    region38.Cities.Add(new City { Index = 162600, Name = "г. Череповец" });
                    regions.Add(region38);

                var region39 = new RegionCity { Name = "Республика Бурятия" };
                    region39.Cities.Add(new City { Index = 671230, Name = "г. Бабушкин" });
                    region39.Cities.Add(new City { Index = 671161, Name = "г. Гусиноозёрск" });
                    region39.Cities.Add(new City { Index = 671950, Name = "г. Закаменск" });
                    region39.Cities.Add(new City { Index = 671840, Name = "г. Кяхта" });
                    region39.Cities.Add(new City { Index = 671700, Name = "г. Северобайкальск" });
                    region39.Cities.Add(new City { Index = 670000, Name = "г. Улан-Удэ" });
                    regions.Add(region39);

                var region40 = new RegionCity { Name = "Калининградская область" };
                    region40.Cities.Add(new City { Index = 238420, Name = "г. Багратионовск" });
                    region40.Cities.Add(new City { Index = 238510, Name = "г. Балтийск" });
                    region40.Cities.Add(new City { Index = 238210, Name = "г. Гвардейск" });
                    region40.Cities.Add(new City { Index = 238300, Name = "г. Гурьевск" });
                    region40.Cities.Add(new City { Index = 238050, Name = "г. Гусев" });
                    region40.Cities.Add(new City { Index = 238410, Name = "г. Железнодорожный" });
                    region40.Cities.Add(new City { Index = 238530, Name = "г. Зеленоградск" });
                    region40.Cities.Add(new City { Index = 236000, Name = "г. Калининград" });
                    region40.Cities.Add(new City { Index = 238730, Name = "г. Краснознаменск" });
                    region40.Cities.Add(new City { Index = 238460, Name = "г. Ладушкин" });
                    region40.Cities.Add(new City { Index = 238450, Name = "г. Мамоново" });
                    region40.Cities.Add(new City { Index = 238710, Name = "г. Неман" });
                    region40.Cities.Add(new City { Index = 238010, Name = "г. Нестеров" });
                    region40.Cities.Add(new City { Index = 238120, Name = "г. Озёрск" });
                    region40.Cities.Add(new City { Index = 238590, Name = "г. Пионерский" });
                    region40.Cities.Add(new City { Index = 238630, Name = "г. Полесск" });
                    region40.Cities.Add(new City { Index = 238400, Name = "г. Правдинск" });
                    region40.Cities.Add(new City { Index = 238510, Name = "г. Приморск" });
                    region40.Cities.Add(new City { Index = 238563, Name = "г. Светлогорск" });
                    region40.Cities.Add(new City { Index = 238340, Name = "г. Светлый" });
                    region40.Cities.Add(new City { Index = 238600, Name = "г. Славск" });
                    region40.Cities.Add(new City { Index = 238750, Name = "г. Советск" });
                    region40.Cities.Add(new City { Index = 238150, Name = "г. Черняховск" });
                    regions.Add(region40);

                var region41 = new RegionCity { Name = "Республика Кабардино-Балкария" };
                    region41.Cities.Add(new City { Index = 361530, Name = "г. Баксан" });
                    region41.Cities.Add(new City { Index = 361115, Name = "г. Майский" });
                    region41.Cities.Add(new City { Index = 360000, Name = "г. Нальчик" });
                    region41.Cities.Add(new City { Index = 361331, Name = "г. Нарткала" });
                    region41.Cities.Add(new City { Index = 361000, Name = "г. Прохладный" });
                    region41.Cities.Add(new City { Index = 361201, Name = "г. Терек" });
                    region41.Cities.Add(new City { Index = 361624, Name = "г. Тырныауз" });
                    region41.Cities.Add(new City { Index = 361510, Name = "г. Чегем" });
                    regions.Add(region41);

                var region42 = new RegionCity { Name = "Калужская область" };
                    region42.Cities.Add(new City { Index = 249001, Name = "г. Балабаново" });
                    region42.Cities.Add(new City { Index = 249160, Name = "г. Белоусово" });
                    region42.Cities.Add(new City { Index = 249010, Name = "г. Боровск" });
                    region42.Cities.Add(new City { Index = 249027, Name = "г. Ермолино" });
                    region42.Cities.Add(new City { Index = 249340, Name = "г. Жиздра" });
                    region42.Cities.Add(new City { Index = 249192, Name = "г. Жуков" });
                    region42.Cities.Add(new City { Index = 248000, Name = "г. Калуга" });
                    region42.Cities.Add(new City { Index = 249440, Name = "г. Киров" });
                    region42.Cities.Add(new City { Index = 249720, Name = "г. Козельск" });
                    region42.Cities.Add(new City { Index = 249831, Name = "г. Кондрово" });
                    region42.Cities.Add(new City { Index = 249185, Name = "г. Кремёнки" });
                    region42.Cities.Add(new City { Index = 249401, Name = "г. Людиново" });
                    region42.Cities.Add(new City { Index = 249091, Name = "г. Малоярославец" });
                    region42.Cities.Add(new City { Index = 249950, Name = "г. Медынь" });
                    region42.Cities.Add(new City { Index = 249240, Name = "г. Мещовск" });
                    region42.Cities.Add(new City { Index = 249930, Name = "г. Мосальск" });
                    region42.Cities.Add(new City { Index = 249030, Name = "г. Обнинск" });
                    region42.Cities.Add(new City { Index = 249711, Name = "г. Сосенский" });
                    region42.Cities.Add(new City { Index = 249610, Name = "г. Спас-Деменск" });
                    region42.Cities.Add(new City { Index = 249275, Name = "г. Сухиничи" });
                    region42.Cities.Add(new City { Index = 249100, Name = "г. Таруса" });
                    region42.Cities.Add(new City { Index = 249910, Name = "г. Юхнов" });
                    regions.Add(region42);

                var region43 = new RegionCity { Name = "Забайкальский край" };
                    region43.Cities.Add(new City { Index = 673450, Name = "г. Балей" });
                    region43.Cities.Add(new City { Index = 674600, Name = "г. Борзя" });
                    region43.Cities.Add(new City { Index = 674676, Name = "г. Краснокаменск" });
                    region43.Cities.Add(new City { Index = 673732, Name = "г. Могоча" });
                    region43.Cities.Add(new City { Index = 673400, Name = "г. Нерчинск" });
                    region43.Cities.Add(new City { Index = 673002, Name = "г. Петровск-Забайкальский" });
                    region43.Cities.Add(new City { Index = 673500, Name = "г. Сретенск" });
                    region43.Cities.Add(new City { Index = 673200, Name = "г. Хилок" });
                    region43.Cities.Add(new City { Index = 672000, Name = "г. Чита" });
                    region43.Cities.Add(new City { Index = 673370, Name = "г. Шилка" });
                    regions.Add(region43);

                var region44 = new RegionCity { Name = "Новосибирская область" };
                    region44.Cities.Add(new City { Index = 632330, Name = "г. Барабинск" });
                    region44.Cities.Add(new City { Index = 633000, Name = "г. Бердск" });
                    region44.Cities.Add(new City { Index = 633340, Name = "г. Болотное" });
                    region44.Cities.Add(new City { Index = 633200, Name = "г. Искитим" });
                    region44.Cities.Add(new City { Index = 632860, Name = "г. Карасук" });
                    region44.Cities.Add(new City { Index = 632402, Name = "г. Каргат" });
                    region44.Cities.Add(new City { Index = 632387, Name = "г. Куйбышев" });
                    region44.Cities.Add(new City { Index = 632740, Name = "г. Купино" });
                    region44.Cities.Add(new City { Index = 630000, Name = "г. Новосибирск" });
                    region44.Cities.Add(new City { Index = 633103, Name = "г. Обь" });
                    region44.Cities.Add(new City { Index = 632120, Name = "г. Татарск" });
                    region44.Cities.Add(new City { Index = 633450, Name = "г. Тогучин" });
                    region44.Cities.Add(new City { Index = 633520, Name = "г. Черепаново" });
                    region44.Cities.Add(new City { Index = 632551, Name = "г. Чулым" });
                    regions.Add(region44);

                var region45 = new RegionCity { Name = "Республика Алтай" };
                    region45.Cities.Add(new City { Index = 649000, Name = "г. Горно-Алтайск" });
                    regions.Add(region45);

                var region46 = new RegionCity { Name = "Ульяновская область" };
                    region46.Cities.Add(new City { Index = 433750, Name = "г. Барыш" });
                    region46.Cities.Add(new City { Index = 433500, Name = "г. Димитровград" });
                    region46.Cities.Add(new City { Index = 433030, Name = "г. Инза" });
                    region46.Cities.Add(new City { Index = 433300, Name = "г. Новоульяновск" });
                    region46.Cities.Add(new City { Index = 433380, Name = "г. Сенгилей" });
                    region46.Cities.Add(new City { Index = 432000, Name = "г. Ульяновск" });
                    regions.Add(region46);

                var region47 = new RegionCity { Name = "Кировская область" };
                    region47.Cities.Add(new City { Index = 613200, Name = "г. Белая Холуница" });
                    region47.Cities.Add(new City { Index = 612960, Name = "г. Вятские Поляны" });
                    region47.Cities.Add(new City { Index = 612410, Name = "г. Зуевка" });
                    region47.Cities.Add(new City { Index = 610000, Name = "г. Киров" });
                    region47.Cities.Add(new City { Index = 613040, Name = "г. Кирово-Чепецк" });
                    region47.Cities.Add(new City { Index = 612820, Name = "г. Кирс" });
                    region47.Cities.Add(new City { Index = 612600, Name = "г. Котельнич" });
                    region47.Cities.Add(new City { Index = 613981, Name = "г. Луза" });
                    region47.Cities.Add(new City { Index = 612920, Name = "г. Малмыж" });
                    region47.Cities.Add(new City { Index = 613710, Name = "г. Мураши" });
                    region47.Cities.Add(new City { Index = 613440, Name = "г. Нолинск" });
                    region47.Cities.Add(new City { Index = 612740, Name = "г. Омутнинск" });
                    region47.Cities.Add(new City { Index = 612270, Name = "г. Орлов" });
                    region47.Cities.Add(new City { Index = 613150, Name = "г. Слободской" });
                    region47.Cities.Add(new City { Index = 613340, Name = "г. Советск" });
                    region47.Cities.Add(new City { Index = 612990, Name = "г. Сосновка" });
                    region47.Cities.Add(new City { Index = 613530, Name = "г. Уржум" });
                    region47.Cities.Add(new City { Index = 612260, Name = "г. Яранск" });
                    regions.Add(region47);

                var region48 = new RegionCity { Name = "Пензенская область" };
                    region48.Cities.Add(new City { Index = 442250, Name = "г. Белинский" });
                    region48.Cities.Add(new City { Index = 442310, Name = "г. Городище" });
                    region48.Cities.Add(new City { Index = 442960, Name = "г. Заречный" });
                    region48.Cities.Add(new City { Index = 442249, Name = "г. Каменка" });
                    region48.Cities.Add(new City { Index = 442530, Name = "г. Кузнецк" });
                    region48.Cities.Add(new City { Index = 442150, Name = "г. Нижний Ломов" });
                    region48.Cities.Add(new City { Index = 442683, Name = "г. Никольск" });
                    region48.Cities.Add(new City { Index = 440000, Name = "г. Пенза" });
                    region48.Cities.Add(new City { Index = 442890, Name = "г. Сердобск" });
                    region48.Cities.Add(new City { Index = 442600, Name = "г. Спасск" });
                    region48.Cities.Add(new City { Index = 442600, Name = "г. Сурск" });
                    regions.Add(region48);

                var region49 = new RegionCity { Name = "Амурская область" };
                    region49.Cities.Add(new City { Index = 676850, Name = "г. Белогорск" });
                    region49.Cities.Add(new City { Index = 675000, Name = "г. Благовещенск" });
                    region49.Cities.Add(new City { Index = 676870, Name = "г. Завитинск" });
                    region49.Cities.Add(new City { Index = 676246, Name = "г. Зея" });
                    region49.Cities.Add(new City { Index = 676770, Name = "г. Райчихинск" });
                    region49.Cities.Add(new City { Index = 676450, Name = "г. Свободный" });
                    region49.Cities.Add(new City { Index = 676010, Name = "г. Сковородино" });
                    region49.Cities.Add(new City { Index = 676282, Name = "г. Тында" });
                    region49.Cities.Add(new City { Index = 676300, Name = "г. Шимановск" });
                    regions.Add(region49);

                var region50 = new RegionCity { Name = "Республика Карелия" };
                    region50.Cities.Add(new City { Index = 186500, Name = "г. Беломорск" });
                    region50.Cities.Add(new City { Index = 186615, Name = "г. Кемь" });
                    region50.Cities.Add(new City { Index = 186220, Name = "г. Кондопога" });
                    region50.Cities.Add(new City { Index = 186930, Name = "г. Костомукша" });
                    region50.Cities.Add(new City { Index = 186730, Name = "г. Лахденпохья" });
                    region50.Cities.Add(new City { Index = 186350, Name = "г. Медвежьегорск" });
                    region50.Cities.Add(new City { Index = 186000, Name = "г. Олонец" });
                    region50.Cities.Add(new City { Index = 185000, Name = "г. Петрозаводск" });
                    region50.Cities.Add(new City { Index = 186810, Name = "г. Питкяранта" });
                    region50.Cities.Add(new City { Index = 186150, Name = "г. Пудож" });
                    region50.Cities.Add(new City { Index = 186420, Name = "г. Сегежа" });
                    region50.Cities.Add(new City { Index = 186790, Name = "г. Сортавала" });
                    region50.Cities.Add(new City { Index = 186870, Name = "г. Суоярви" });
                    regions.Add(region50);

                var region51 = new RegionCity { Name = "Еврейская автономная область" };
                    region51.Cities.Add(new City { Index = 679000, Name = "г. Биробиджан" });
                    region51.Cities.Add(new City { Index = 679100, Name = "г. Облучье" });
                    regions.Add(region51);

                var region52 = new RegionCity { Name = "Ставропольский край" };
                    region52.Cities.Add(new City { Index = 356420, Name = "г. Благодарный" });
                    region52.Cities.Add(new City { Index = 356800, Name = "г. Будённовск" });
                    region52.Cities.Add(new City { Index = 357820, Name = "г. Георгиевск" });
                    region52.Cities.Add(new City { Index = 357601, Name = "г. Ессентуки" });
                    region52.Cities.Add(new City { Index = 357400, Name = "г. Железноводск" });
                    region52.Cities.Add(new City { Index = 357910, Name = "г. Зеленокумск" });
                    region52.Cities.Add(new City { Index = 356140, Name = "г. Изобильный" });
                    region52.Cities.Add(new City { Index = 356630, Name = "г. Ипатово" });
                    region52.Cities.Add(new City { Index = 357700, Name = "г. Кисловодск" });
                    region52.Cities.Add(new City { Index = 357340, Name = "г. Лермонтов" });
                    region52.Cities.Add(new City { Index = 357200, Name = "г. Минеральные Воды" });
                    region52.Cities.Add(new City { Index = 356240, Name = "г. Михайловск" });
                    region52.Cities.Add(new City { Index = 357100, Name = "г. Невинномысск" });
                    region52.Cities.Add(new City { Index = 356880, Name = "г. Нефтекумск" });
                    region52.Cities.Add(new City { Index = 356000, Name = "г. Новоалександровск" });
                    region52.Cities.Add(new City { Index = 357300, Name = "г. Новопавловск" });
                    region52.Cities.Add(new City { Index = 357500, Name = "г. Пятигорск" });
                    region52.Cities.Add(new City { Index = 356530, Name = "г. Светлоград" });
                    region52.Cities.Add(new City { Index = 355000, Name = "г. Ставрополь" });
                    regions.Add(region52);

                var region53 = new RegionCity { Name = "Воронежская область" };
                    region53.Cities.Add(new City { Index = 397700, Name = "г. Бобров" });
                    region53.Cities.Add(new City { Index = 396790, Name = "г. Богучар" });
                    region53.Cities.Add(new City { Index = 397160, Name = "г. Борисоглебск" });
                    region53.Cities.Add(new City { Index = 397500, Name = "г. Бутурлиновка" });
                    region53.Cities.Add(new City { Index = 394000, Name = "г. Воронеж" });
                    region53.Cities.Add(new City { Index = 397600, Name = "г. Калач" });
                    region53.Cities.Add(new City { Index = 397900, Name = "г. Лиски" });
                    region53.Cities.Add(new City { Index = 396070, Name = "г. Нововоронеж" });
                    region53.Cities.Add(new City { Index = 397400, Name = "г. Новохопёрск" });
                    region53.Cities.Add(new City { Index = 397852, Name = "г. Острогожск" });
                    region53.Cities.Add(new City { Index = 396420, Name = "г. Павловск" });
                    region53.Cities.Add(new City { Index = 397350, Name = "г. Поворино" });
                    region53.Cities.Add(new City { Index = 396650, Name = "г. Россошь" });
                    region53.Cities.Add(new City { Index = 396900, Name = "г. Семилуки" });
                    region53.Cities.Add(new City { Index = 397030, Name = "г. Эртиль" });
                    regions.Add(region53);

                var region54 = new RegionCity { Name = "Ленинградская область" };
                    region54.Cities.Add(new City { Index = 187650, Name = "г. Бокситогорск" });
                    region54.Cities.Add(new City { Index = 188410, Name = "г. Волосово" });
                    region54.Cities.Add(new City { Index = 187400, Name = "г. Волхов" });
                    region54.Cities.Add(new City { Index = 188640, Name = "г. Всеволожск" });
                    region54.Cities.Add(new City { Index = 188800, Name = "г. Выборг" });
                    region54.Cities.Add(new City { Index = 188909, Name = "г. Высоцк" });
                    region54.Cities.Add(new City { Index = 188300, Name = "г. Гатчина" });
                    region54.Cities.Add(new City { Index = 188490, Name = "г. Ивангород" });
                    region54.Cities.Add(new City { Index = 188950, Name = "г. Каменногорск" });
                    region54.Cities.Add(new City { Index = 188455, Name = "г. Кингисепп" });
                    region54.Cities.Add(new City { Index = 187110, Name = "г. Кириши" });
                    region54.Cities.Add(new City { Index = 187341, Name = "г. Кировск" });
                    region54.Cities.Add(new City { Index = 188320, Name = "г. Коммунар" });
                    region54.Cities.Add(new City { Index = 187700, Name = "г. Лодейное Поле" });
                    region54.Cities.Add(new City { Index = 188230, Name = "г. Луга" });
                    region54.Cities.Add(new City { Index = 187050, Name = "г. Любань" });
                    region54.Cities.Add(new City { Index = 187026, Name = "г. Никольское" });
                    region54.Cities.Add(new City { Index = 187450, Name = "г. Новая Ладога" });
                    region54.Cities.Add(new City { Index = 187330, Name = "г. Отрадное" });
                    region54.Cities.Add(new City { Index = 190000, Name = "г. Санкт-Петербург" });
                    region54.Cities.Add(new City { Index = 187600, Name = "г. Пикалёво" });
                    region54.Cities.Add(new City { Index = 187780, Name = "г. Подпорожье" });
                    region54.Cities.Add(new City { Index = 188760, Name = "г. Приозерск" });
                    region54.Cities.Add(new City { Index = 188990, Name = "г. Светогорск" });
                    region54.Cities.Add(new City { Index = 188650, Name = "г. Сертолово" });
                    region54.Cities.Add(new City { Index = 188560, Name = "г. Сланцы" });
                    region54.Cities.Add(new City { Index = 188540, Name = "г. Сосновый Бор" });
                    region54.Cities.Add(new City { Index = 187420, Name = "г. Сясьстрой" });
                    region54.Cities.Add(new City { Index = 187551, Name = "г. Тихвин" });
                    region54.Cities.Add(new City { Index = 187000, Name = "г. Тосно" });
                    region54.Cities.Add(new City { Index = 187320, Name = "г. Шлиссельбург" });
                    regions.Add(region54);

                var region55 = new RegionCity { Name = "Орловская область" };
                    region55.Cities.Add(new City { Index = 303140, Name = "г. Болхов" });
                    region55.Cities.Add(new City { Index = 303240, Name = "г. Дмитровск" });
                    region55.Cities.Add(new City { Index = 303850, Name = "г. Ливны" });
                    region55.Cities.Add(new City { Index = 303370, Name = "г. Малоархангельск" });
                    region55.Cities.Add(new City { Index = 303030, Name = "г. Мценск" });
                    region55.Cities.Add(new City { Index = 303500, Name = "г. Новосиль" });
                    region55.Cities.Add(new City { Index = 302000, Name = "г. Орёл" });
                    regions.Add(region55);

                var region56 = new RegionCity { Name = "Новгородская область" };
                    region56.Cities.Add(new City { Index = 174401, Name = "г. Боровичи" });
                    region56.Cities.Add(new City { Index = 175400, Name = "г. Валдай" });
                    region56.Cities.Add(new City { Index = 173000, Name = "г. Великий Новгород" });
                    region56.Cities.Add(new City { Index = 174260, Name = "г. Малая Вишера" });
                    region56.Cities.Add(new City { Index = 174350, Name = "г. Окуловка" });
                    region56.Cities.Add(new City { Index = 174510, Name = "г. Пестово" });
                    region56.Cities.Add(new City { Index = 175040, Name = "г. Сольцы" });
                    region56.Cities.Add(new City { Index = 175200, Name = "г. Старая Русса" });
                    region56.Cities.Add(new City { Index = 175270, Name = "г. Холм" });
                    region56.Cities.Add(new City { Index = 174210, Name = "г. Чудово    " });
                    regions.Add(region56);

                var region57 = new RegionCity { Name = "Брянская область" };
                    region57.Cities.Add(new City { Index = 241000, Name = "г. Брянск" });
                    region57.Cities.Add(new City { Index = 242600, Name = "г. Дятьково" });
                    region57.Cities.Add(new City { Index = 242700, Name = "г. Жуковка" });
                    region57.Cities.Add(new City { Index = 243600, Name = "г. Злынка" });
                    region57.Cities.Add(new City { Index = 242500, Name = "г. Карачев" });
                    region57.Cities.Add(new City { Index = 243140, Name = "г. Клинцы" });
                    region57.Cities.Add(new City { Index = 243220, Name = "г. Мглин" });
                    region57.Cities.Add(new City { Index = 243020, Name = "г. Новозыбков" });
                    region57.Cities.Add(new City { Index = 243400, Name = "г. Почеп" });
                    region57.Cities.Add(new City { Index = 242440, Name = "г. Севск" });
                    region57.Cities.Add(new City { Index = 241550, Name = "г. Сельцо" });
                    region57.Cities.Add(new City { Index = 243240, Name = "г. Стародуб" });
                    region57.Cities.Add(new City { Index = 243500, Name = "г. Сураж" });
                    region57.Cities.Add(new City { Index = 242220, Name = "г. Трубчевск" });
                    region57.Cities.Add(new City { Index = 243300, Name = "г. Унеча" });
                    region57.Cities.Add(new City { Index = 242610, Name = "г. Фокино" });
                    regions.Add(region57);

                var region58 = new RegionCity { Name = "Костромская область" };
                    region58.Cities.Add(new City { Index = 157000, Name = "г. Буй" });
                    region58.Cities.Add(new City { Index = 156901, Name = "г. Волгореченск" });
                    region58.Cities.Add(new City { Index = 157202, Name = "г. Галич" });
                    region58.Cities.Add(new City { Index = 157440, Name = "г. Кологрив" });
                    region58.Cities.Add(new City { Index = 156000, Name = "г. Кострома" });
                    region58.Cities.Add(new City { Index = 157460, Name = "г. Макарьев" });
                    region58.Cities.Add(new City { Index = 157300, Name = "г. Мантурово" });
                    region58.Cities.Add(new City { Index = 157800, Name = "г. Нерехта" });
                    region58.Cities.Add(new City { Index = 157330, Name = "г. Нея" });
                    region58.Cities.Add(new City { Index = 157170, Name = "г. Солигалич" });
                    region58.Cities.Add(new City { Index = 157130, Name = "г. Чухлома" });
                    region58.Cities.Add(new City { Index = 157500, Name = "г. Шарья" });
                    regions.Add(region58);

                var region59 = new RegionCity { Name = "Республика Дагестан" };
                    region59.Cities.Add(new City { Index = 368220, Name = "г. Буйнакск" });
                    region59.Cities.Add(new City { Index = 368670, Name = "г. Дагестанские Огни" });
                    region59.Cities.Add(new City { Index = 368600, Name = "г. Дербент" });
                    region59.Cities.Add(new City { Index = 368500, Name = "г. Избербаш" });
                    region59.Cities.Add(new City { Index = 377563, Name = "г. Каспийск" });
                    region59.Cities.Add(new City { Index = 368124, Name = "г. Кизилюрт" });
                    region59.Cities.Add(new City { Index = 368810, Name = "г. Кизляр" });
                    region59.Cities.Add(new City { Index = 367000, Name = "г. Махачкала" });
                    region59.Cities.Add(new City { Index = 368000, Name = "г. Хасавюрт" });
                    region59.Cities.Add(new City { Index = 368890, Name = "г. Южно-Сухокумск" });
                    regions.Add(region59);

                var region60 = new RegionCity { Name = "Смоленская область" };
                    region60.Cities.Add(new City { Index = 216290, Name = "г. Велиж" });
                    region60.Cities.Add(new City { Index = 215100, Name = "г. Вязьма" });
                    region60.Cities.Add(new City { Index = 215010, Name = "г. Гагарин" });
                    region60.Cities.Add(new City { Index = 216240, Name = "г. Демидов" });
                    region60.Cities.Add(new City { Index = 216400, Name = "г. Десногорск" });
                    region60.Cities.Add(new City { Index = 215710, Name = "г. Дорогобуж" });
                    region60.Cities.Add(new City { Index = 216200, Name = "г. Духовщина" });
                    region60.Cities.Add(new City { Index = 216330, Name = "г. Ельня" });
                    region60.Cities.Add(new City { Index = 216450, Name = "г. Починок" });
                    region60.Cities.Add(new City { Index = 216500, Name = "г. Рославль" });
                    region60.Cities.Add(new City { Index = 216790, Name = "г. Рудня" });
                    region60.Cities.Add(new City { Index = 215500, Name = "г. Сафоново" });
                    region60.Cities.Add(new City { Index = 214000, Name = "г. Смоленск" });
                    region60.Cities.Add(new City { Index = 215280, Name = "г. Сычевка" });
                    region60.Cities.Add(new City { Index = 215800, Name = "г. Ярцево" });
                    regions.Add(region60);

                var region61 = new RegionCity { Name = "Псковская область" };
                    region61.Cities.Add(new City { Index = 182100, Name = "г. Великие Луки" });
                    region61.Cities.Add(new City { Index = 181600, Name = "г. Гдов" });
                    region61.Cities.Add(new City { Index = 182670, Name = "г. Дно" });
                    region61.Cities.Add(new City { Index = 182500, Name = "г. Невель" });
                    region61.Cities.Add(new City { Index = 182440, Name = "г. Новоржев" });
                    region61.Cities.Add(new City { Index = 182200, Name = "г. Новосокольники" });
                    region61.Cities.Add(new City { Index = 182330, Name = "г. Опочка" });
                    region61.Cities.Add(new City { Index = 181350, Name = "г. Остров" });
                    region61.Cities.Add(new City { Index = 181500, Name = "г. Печоры" });
                    region61.Cities.Add(new City { Index = 182620, Name = "г. Порхов" });
                    region61.Cities.Add(new City { Index = 180000, Name = "г. Псков" });
                    region61.Cities.Add(new City { Index = 182300, Name = "г. Пустошка" });
                    region61.Cities.Add(new City { Index = 181410, Name = "г. Пыталово" });
                    region61.Cities.Add(new City { Index = 182250, Name = "г. Себеж" });
                    regions.Add(region61);

                var region62 = new RegionCity { Name = "Ивановская область" };
                    region62.Cities.Add(new City { Index = 155330, Name = "г. Вичуга" });
                    region62.Cities.Add(new City { Index = 155000, Name = "г. Гаврилов Посад" });
                    region62.Cities.Add(new City { Index = 155410, Name = "г. Заволжск" });
                    region62.Cities.Add(new City { Index = 153000, Name = "г. Иваново" });
                    region62.Cities.Add(new City { Index = 155800, Name = "г. Кинешма" });
                    region62.Cities.Add(new City { Index = 155150, Name = "г. Комсомольск" });
                    region62.Cities.Add(new City { Index = 153510, Name = "г. Кохма" });
                    region62.Cities.Add(new City { Index = 155830, Name = "г. Наволоки" });
                    region62.Cities.Add(new City { Index = 155555, Name = "г. Плёс" });
                    region62.Cities.Add(new City { Index = 155550, Name = "г. Приволжск" });
                    region62.Cities.Add(new City { Index = 155360, Name = "г. Пучеж" });
                    region62.Cities.Add(new City { Index = 155250, Name = "г. Родники" });
                    region62.Cities.Add(new City { Index = 155040, Name = "г. Тейково" });
                    region62.Cities.Add(new City { Index = 155520, Name = "г. Фурманов" });
                    region62.Cities.Add(new City { Index = 155900, Name = "г. Шуя" });
                    region62.Cities.Add(new City { Index = 155630, Name = "г. Южа" });
                    region62.Cities.Add(new City { Index = 155453, Name = "г. Юрьевец" });
                    regions.Add(region62);

                var region63 = new RegionCity { Name = "Республика Марий Эл" };
                    region63.Cities.Add(new City { Index = 425000, Name = "г. Волжск" });
                    region63.Cities.Add(new City { Index = 425060, Name = "г. Звенигово" });
                    region63.Cities.Add(new City { Index = 424000, Name = "г. Йошкар-Ола" });
                    region63.Cities.Add(new City { Index = 425350, Name = "г. Козьмодемьянск" });
                    regions.Add(region63);

                var region64 = new RegionCity { Name = "Волгоградская область" };
                    region64.Cities.Add(new City { Index = 400000, Name = "г. Волгоград" });
                    region64.Cities.Add(new City { Index = 404100, Name = "г. Волжский" });
                    region64.Cities.Add(new City { Index = 403000, Name = "г.п. Городище" });
                    region64.Cities.Add(new City { Index = 404002, Name = "г. Дубовка" });
                    region64.Cities.Add(new City { Index = 403790, Name = "г. Жирновск" });
                    region64.Cities.Add(new City { Index = 404507, Name = "г. Калач-на-Дону" });
                    region64.Cities.Add(new City { Index = 403870, Name = "г. Камышин" });
                    region64.Cities.Add(new City { Index = 404352, Name = "г. Котельниково" });
                    region64.Cities.Add(new City { Index = 403805, Name = "г. Котово" });
                    region64.Cities.Add(new City { Index = 404160, Name = "г. Краснослободск" });
                    region64.Cities.Add(new City { Index = 404620, Name = "г. Ленинск" });
                    region64.Cities.Add(new City { Index = 403300, Name = "г. Михайловка" });
                    region64.Cities.Add(new City { Index = 404030, Name = "г. Николаевск" });
                    region64.Cities.Add(new City { Index = 161440, Name = "г. Никольск" });
                    region64.Cities.Add(new City { Index = 403950, Name = "г. Новоаннинский" });
                    region64.Cities.Add(new City { Index = 404263, Name = "г. Палласовка" });
                    region64.Cities.Add(new City { Index = 403840, Name = "г. Петров Вал" });
                    region64.Cities.Add(new City { Index = 403441, Name = "г. Серафимович" });
                    region64.Cities.Add(new City { Index = 404411, Name = "г. Суровикино" });
                    region64.Cities.Add(new City { Index = 403120, Name = "г. Урюпинск" });
                    region64.Cities.Add(new City { Index = 403530, Name = "г. Фролово" });
                    regions.Add(region64);

                var region65 = new RegionCity { Name = "Республика Коми" };
                    region65.Cities.Add(new City { Index = 169900, Name = "г. Воркута" });
                    region65.Cities.Add(new City { Index = 169570, Name = "г. Вуктыл" });
                    region65.Cities.Add(new City { Index = 169200, Name = "г. Емва" });
                    region65.Cities.Add(new City { Index = 169840, Name = "г. Инта" });
                    region65.Cities.Add(new City { Index = 169060, Name = "г. Микунь" });
                    region65.Cities.Add(new City { Index = 169601, Name = "г. Печора" });
                    region65.Cities.Add(new City { Index = 169500, Name = "г. Сосногорск" });
                    region65.Cities.Add(new City { Index = 167000, Name = "г. Сыктывкар" });
                    region65.Cities.Add(new City { Index = 169710, Name = "г. Усинск" });
                    region65.Cities.Add(new City { Index = 169300, Name = "г. Ухта" });
                    regions.Add(region65);

                var region66 = new RegionCity { Name = "Удмуртская республика" };
                    region66.Cities.Add(new City { Index = 427430, Name = "г. Воткинск" });
                    region66.Cities.Add(new City { Index = 427620, Name = "г. Глазов" });
                    region66.Cities.Add(new City { Index = 426000, Name = "г. Ижевск" });
                    region66.Cities.Add(new City { Index = 427950, Name = "г. Камбарка" });
                    region66.Cities.Add(new City { Index = 427789, Name = "г. Можга" });
                    region66.Cities.Add(new City { Index = 427960, Name = "г. Сарапул" });
                    regions.Add(region66);

                var region67 = new RegionCity { Name = "Ярославская область" };
                    region67.Cities.Add(new City { Index = 152240, Name = "г. Гаврилов-Ям" });
                    region67.Cities.Add(new City { Index = 152070, Name = "г. Данилов" });
                    region67.Cities.Add(new City { Index = 152470, Name = "г. Любим" });
                    region67.Cities.Add(new City { Index = 152830, Name = "г. Мышкин" });
                    region67.Cities.Add(new City { Index = 152020, Name = "г. Переславль-Залесский" });
                    region67.Cities.Add(new City { Index = 152850, Name = "г. Пошехонье" });
                    region67.Cities.Add(new City { Index = 152150, Name = "г. Ростов Великий" });
                    region67.Cities.Add(new City { Index = 152900, Name = "г. Рыбинск" });
                    region67.Cities.Add(new City { Index = 152300, Name = "г. Тутаев" });
                    region67.Cities.Add(new City { Index = 152610, Name = "г. Углич" });
                    region67.Cities.Add(new City { Index = 150000, Name = "г. Ярославль" });
                    regions.Add(region67);

                var region68 = new RegionCity { Name = "Республика Калмыкия" };
                    region68.Cities.Add(new City { Index = 359050, Name = "г. Городовиковск" });
                    region68.Cities.Add(new City { Index = 359221, Name = "г. Лагань" });
                    region68.Cities.Add(new City { Index = 358000, Name = "г. Элиста" });
                    regions.Add(region68);

                var region69 = new RegionCity { Name = "Липецкая область" };
                    region69.Cities.Add(new City { Index = 399050, Name = "г. Грязи" });
                    region69.Cities.Add(new City { Index = 399851, Name = "г. Данков" });
                    region69.Cities.Add(new City { Index = 399770, Name = "г. Елец" });
                    region69.Cities.Add(new City { Index = 399200, Name = "г. Задонск" });
                    region69.Cities.Add(new City { Index = 399610, Name = "г. Лебедянь" });
                    region69.Cities.Add(new City { Index = 398000, Name = "г. Липецк" });
                    region69.Cities.Add(new City { Index = 399370, Name = "г. Усмань" });
                    region69.Cities.Add(new City { Index = 399900, Name = "г. Чаплыгин" });
                    regions.Add(region69);

                var region70 = new RegionCity { Name = "Ямало-Ненецкий автономный округ" };
                    region70.Cities.Add(new City { Index = 629830, Name = "г. Губкинский" });
                    region70.Cities.Add(new City { Index = 629400, Name = "г. Лабытнанги" });
                    region70.Cities.Add(new City { Index = 629600, Name = "г. Муравленко" });
                    region70.Cities.Add(new City { Index = 629730, Name = "г. Надым" });
                    region70.Cities.Add(new City { Index = 629300, Name = "г. Новый Уренгой" });
                    region70.Cities.Add(new City { Index = 629800, Name = "г. Ноябрьск" });
                    region70.Cities.Add(new City { Index = 629000, Name = "г. Салехард" });
                    regions.Add(region70);

                var region71 = new RegionCity { Name = "Курганская область" };
                    region71.Cities.Add(new City { Index = 641730, Name = "г. Далматово" });
                    region71.Cities.Add(new City { Index = 641700, Name = "г. Катайск" });
                    region71.Cities.Add(new City { Index = 640000, Name = "г. Курган" });
                    region71.Cities.Add(new City { Index = 641430, Name = "г. Куртамыш" });
                    region71.Cities.Add(new City { Index = 641600, Name = "г. Макушино" });
                    region71.Cities.Add(new City { Index = 641640, Name = "г. Петухово" });
                    region71.Cities.Add(new City { Index = 641870, Name = "г. Шадринск" });
                    region71.Cities.Add(new City { Index = 641100, Name = "г. Шумиха" });
                    region71.Cities.Add(new City { Index = 641010, Name = "г. Щучье" });
                    regions.Add(region71);

                var region72 = new RegionCity { Name = "Курская область" };
                    region72.Cities.Add(new City { Index = 307500, Name = "г. Дмитриев-Льговский" });
                    region72.Cities.Add(new City { Index = 307170, Name = "г. Железногорск" });
                    region72.Cities.Add(new City { Index = 305000, Name = "г. Курск" });
                    region72.Cities.Add(new City { Index = 307250, Name = "г. Курчатов" });
                    region72.Cities.Add(new City { Index = 307751, Name = "г. Льгов" });
                    region72.Cities.Add(new City { Index = 306230, Name = "г. Обоянь" });
                    region72.Cities.Add(new City { Index = 307370, Name = "г. Рыльск" });
                    region72.Cities.Add(new City { Index = 307800, Name = "г. Суджа" });
                    region72.Cities.Add(new City { Index = 307100, Name = "г. Фатеж" });
                    region72.Cities.Add(new City { Index = 306530, Name = "г. Щигры" });
                    regions.Add(region72);

                var region73 = new RegionCity { Name = "Тамбовская область" };
                    region73.Cities.Add(new City { Index = 393670, Name = "г. Жердевка" });
                    region73.Cities.Add(new City { Index = 393360, Name = "г. Кирсанов" });
                    region73.Cities.Add(new City { Index = 393194, Name = "г. Котовск" });
                    region73.Cities.Add(new City { Index = 393760, Name = "г. Мичуринск" });
                    region73.Cities.Add(new City { Index = 393950, Name = "г. Моршанск" });
                    region73.Cities.Add(new City { Index = 393250, Name = "г. Рассказово" });
                    region73.Cities.Add(new City { Index = 393840, Name = "п.г.т. Сосновка" });
                    region73.Cities.Add(new City { Index = 392000, Name = "г. Тамбов" });
                    region73.Cities.Add(new City { Index = 393460, Name = "г. Уварово" });
                    regions.Add(region73);

                var region74 = new RegionCity { Name = "Самарская область" };
                    region74.Cities.Add(new City { Index = 445350, Name = "г. Жигулёвск" });
                    region74.Cities.Add(new City { Index = 446430, Name = "г. Кинель" });
                    region74.Cities.Add(new City { Index = 446600, Name = "г. Нефтегорск" });
                    region74.Cities.Add(new City { Index = 446200, Name = "г. Новокуйбышевск" });
                    region74.Cities.Add(new City { Index = 445240, Name = "г. Октябрьск" });
                    region74.Cities.Add(new City { Index = 446300, Name = "г. Отрадный" });
                    region74.Cities.Add(new City { Index = 446450, Name = "г. Похвистнево" });
                    region74.Cities.Add(new City { Index = 443000, Name = "г. Самара" });
                    region74.Cities.Add(new City { Index = 446000, Name = "г. Сызрань" });
                    region74.Cities.Add(new City { Index = 445000, Name = "г. Тольятти" });
                    region74.Cities.Add(new City { Index = 446100, Name = "г. Чапаевск" });
                    regions.Add(region74);

                var region75 = new RegionCity { Name = "Тюменская область" };
                    region75.Cities.Add(new City { Index = 627140, Name = "г. Заводоуковск" });
                    region75.Cities.Add(new City { Index = 627750, Name = "г. Ишим" });
                    region75.Cities.Add(new City { Index = 626128, Name = "г. Тобольск" });
                    region75.Cities.Add(new City { Index = 625000, Name = "г. Тюмень" });
                    region75.Cities.Add(new City { Index = 627010, Name = "г. Ялуторовск" });
                    regions.Add(region75);

                var region76 = new RegionCity { Name = "Омская область" };
                    region76.Cities.Add(new City { Index = 646020, Name = "г. Исилькуль" });
                    region76.Cities.Add(new City { Index = 646900, Name = "г. Калачинск" });
                    region76.Cities.Add(new City { Index = 646100, Name = "г. Называевск" });
                    region76.Cities.Add(new City { Index = 644000, Name = "г. Омск" });
                    region76.Cities.Add(new City { Index = 646530, Name = "г. Тара" });
                    region76.Cities.Add(new City { Index = 646330, Name = "г. Тюкалинск" });
                    regions.Add(region76);

                var region77 = new RegionCity { Name = "Республика Ингушетия" };
                    region77.Cities.Add(new City { Index = 386230, Name = "г. Карабулак" });
                    region77.Cities.Add(new City { Index = 386001, Name = "г. Магас" });
                    region77.Cities.Add(new City { Index = 386300, Name = "г. Малгобек" });
                    region77.Cities.Add(new City { Index = 386120, Name = "г. Назрань" });
                    regions.Add(region77);

                var region78 = new RegionCity { Name = "Карачаево-Черкесская республика" };
                    region78.Cities.Add(new City { Index = 369200, Name = "г. Карачаевск" });
                    region78.Cities.Add(new City { Index = 369210, Name = "г. Теберда" });
                    region78.Cities.Add(new City { Index = 369300, Name = "г. Усть-Джегута" });
                    region78.Cities.Add(new City { Index = 369000, Name = "г. Черкесск" });
                    regions.Add(region78);

                var region79 = new RegionCity { Name = "Рязанская область" };
                    region79.Cities.Add(new City { Index = 391300, Name = "г. Касимов" });
                    region79.Cities.Add(new City { Index = 391200, Name = "г. Кораблино" });
                    region79.Cities.Add(new City { Index = 391710, Name = "г. Михайлов" });
                    region79.Cities.Add(new City { Index = 391160, Name = "г. Новомичуринск" });
                    region79.Cities.Add(new City { Index = 391110, Name = "г. Рыбное" });
                    region79.Cities.Add(new City { Index = 391960, Name = "г. Ряжск" });
                    region79.Cities.Add(new City { Index = 390000, Name = "г. Рязань" });
                    region79.Cities.Add(new City { Index = 391430, Name = "г. Сасово" });
                    region79.Cities.Add(new City { Index = 391800, Name = "г. Скопин" });
                    region79.Cities.Add(new City { Index = 391030, Name = "г. Спас-Клепики" });
                    region79.Cities.Add(new City { Index = 391050, Name = "г. Спасск-Рязанский" });
                    region79.Cities.Add(new City { Index = 391550, Name = "г. Шацк" });
                    regions.Add(region79);

                var region80 = new RegionCity { Name = "Магаданская область" };
                    region80.Cities.Add(new City { Index = 685000, Name = "г. Магадан" });
                    region80.Cities.Add(new City { Index = 686310, Name = "г. Сусуман" });
                    regions.Add(region80);

                var region81 = new RegionCity { Name = "Ненецкий автономный округ" };
                    region81.Cities.Add(new City { Index = 166000, Name = "г. Нарьян-Мар" });
                    regions.Add(region81);

                db.Regions.AddRange(regions);
            }

            string pass = "GoodJokerCfvsqGjgekzhysqGhjtrn",
                   tatrPass = "123321";
            pass = Hash.Get(pass);
            tatrPass = Hash.Get(tatrPass);

            var admin = new User
            {
                Email = "gurumaiba@gmail.com",
                Nick = "GoodJoker",
                HashPass = pass,
                Role = role1,
                ConfirmEmail = true,
                LastOnline = DateTime.UtcNow,
                DateRegistration = DateTime.UtcNow,
                Option = new OptionUser()
                {
                    Ava = "defaultAva.jpg",
                    Birthday = DateTime.Now,
                    BigCity = defaultCity,
                    City = defaultCity
                }
            };

            var support = new User
            {
                Email = "team@goodjoker.ru",
                Nick = "Support",
                HashPass = pass,
                Role = role1,
                ConfirmEmail = true,
                LastOnline = DateTime.UtcNow,
                DateRegistration = DateTime.UtcNow,
                Option = new OptionUser()
                {
                    Ava = "defaultAva.jpg",
                    Birthday = DateTime.Now,
                    BigCity = defaultCity,
                    City = defaultCity
                }
            };

            var team = new StudioTeam()
            {
                ConfirmMember = true,
                Member = admin,
                Role = new List<StudioRoleMember> { roleStudio }
            };

            var mamba = new User
            {
                Email = "Xloridomanad@yandex.ru",
                Nick = "Mamba",
                HashPass = tatrPass,
                Role = role2,
                ConfirmEmail = true,
                LastOnline = DateTime.UtcNow,
                DateRegistration = DateTime.UtcNow,
                Option = new OptionUser()
                {
                    Ava = "defaultAva.jpg",
                    Birthday = DateTime.Now,
                    BigCity = defaultCity,
                    City = defaultCity
                }
            };

            var ara = new User
            {
                Email = "Ilnur1512@yandex.ru",
                Nick = "DIM[GJ]",
                HashPass = tatrPass,
                Role = role2,
                ConfirmEmail = true,
                LastOnline = DateTime.UtcNow,
                DateRegistration = DateTime.UtcNow,
                Option = new OptionUser()
                {
                    Ava = "defaultAva.jpg",
                    Birthday = DateTime.Now,
                    BigCity = defaultCity,
                    City = defaultCity
                }
            };

            //var artem = new User
            //{
            //    Email = "skakunov1707@gmail.com",
            //    Nick = "Tema",
            //    HashPass = tatrPass,
            //    Role = role1,
            //    ConfirmEmail = true,
            //    LastOnline = DateTime.UtcNow,
            //    DateRegistration = DateTime.UtcNow,
            //    Option = new OptionUser()
            //    {
            //        Ava = "defaultAva.jpg",
            //        Birthday = DateTime.Now,
            //        BigCity = defaultCity,
            //        City = defaultCity
            //    }
            //};

            var newStudio = new Studio()
            {
                Name = "GoodJoker",
                DateCreate = DateTime.UtcNow,
                Option = new OptionStudio() {
                    Ava = "defaultAva.jpg",
                    Cover = "defaultCover.jpg",
                    BeginVIP = DateTime.Now,
                    EndVIP = DateTime.Now
                },
                Team = new List<StudioTeam>() { team }
            };
            
            studRoles.Add(new StudioRoleMember { Name = "Admin" });
            studRoles.Add(new StudioRoleMember { Name = "Member" });
            studRoles.Add(new StudioRoleMember { Name = "Sub" });
            studRoles.Add(new StudioRoleMember { Name = "ViewTeam" });
            studRoles.Add(new StudioRoleMember { Name = "EditInfo" });
            studRoles.Add(new StudioRoleMember { Name = "CreateLottery" });
            studRoles.Add(new StudioRoleMember { Name = "CommentStudio" });

            db.Roles.AddRange(roles);
            db.Users.Add(admin);
            db.Users.Add(support);
            db.Users.Add(mamba);
            db.Users.Add(ara);
            db.Studios.Add(newStudio);
            db.StudioRolesMember.AddRange(studRoles);

            db.InfoSite.Add(new InfoSite { Token = Hash.Get("AboutSite"), CountRegUser = 5, CountArchLott = 0, AverageAssess = 0.01, CountAssessUser = 0 });

            db.SaveChanges();
        }
    }
}