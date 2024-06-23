namespace ApplicationLLA.Server.Helper
{
    public static class ArgumentLists
    {
        public static List<string> GetCurrencies()
        {
            return new List<string>
                {
            "USD-$",
            "EUR-€",
            "TRY-₺",
            "JPY-¥",
            "GBP-£",
            "AUD-A$",
            "CAD-C$",
            "CHF-CHF",
            "CNY-¥",
            "HKD-HK$",
            "SGD-S$",
            "SEK-kr",
            "KRW-₩",
            "NOK-kr",
            "NZD-NZ$",
            "INR-₹",
            "MXN-MX$",
            "TWD-NT$",
            "ZAR-R",
            "BRL-R$"
             };
        }

        public static List<string> GetWorkUnits()
        {
            return new List<string>
            {
                    "1h",
                    "0.5h",
                    "1 day",
                    "1 week",
                    "1 month",
                    "1 room",
                    "m²",
                    "1 meter",
                    "10 meter",
                    "100 meter",
                    "1-km",
                    "10-km",
                    "100-km",
                    "event",
                    "other"
            };
        }
        public static List<string> GetCities()
        {
            return new List<string>
            {
                "Adana",
                "Adıyaman",
                "Afyonkarahisar",
                "Ağrı",
                "Aksaray",
                "Amasya",
                "Ankara",
                "Antalya",
                "Ardahan",
                "Artvin",
                "Aydın",
                "Balıkesir",
                "Bartın",
                "Batman",
                "Bayburt",
                "Bilecik",
                "Bingöl",
                "Bitlis",
                "Bolu",
                "Burdur",
                "Bursa",
                "Çanakkale",
                "Çankırı",
                "Çorum",
                "Denizli",
                "Diyarbakır",
                "Düzce",
                "Edirne",
                "Elazığ",
                "Erzincan",
                "Erzurum",
                "Eskişehir",
                "Gaziantep",
                "Giresun",
                "Gümüşhane",
                "Hakkâri",
                "Hatay",
                "Iğdır",
                "Isparta",
                "İstanbul",
                "İzmir",
                "Kahramanmaraş",
                "Karabük",
                "Karaman",
                "Kars",
                "Kastamonu",
                "Kayseri",
                "Kırıkkale",
                "Kırklareli",
                "Kırşehir",
                "Kilis",
                "Kocaeli",
                "Konya",
                "Kütahya",
                "Malatya",
                "Manisa",
                "Mardin",
                "Mersin",
                "Muğla",
                "Muş",
                "Nevşehir",
                "Niğde",
                "Ordu",
                "Osmaniye",
                "Rize",
                "Sakarya",
                "Samsun",
                "Siirt",
                "Sinop",
                "Sivas",
                "Şanlıurfa",
                "Şırnak",
                "Tekirdağ",
                "Tokat",
                "Trabzon",
                "Tunceli",
                "Uşak",
                "Van",
                "Yalova",
                "Yozgat",
                "Zonguldak",

            };
        }
    }
}
