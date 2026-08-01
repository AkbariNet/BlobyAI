using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlobyAI.Properties
{
    public static class MainLanguage
    {
        public enum Language
        {
            EN,
            FA
        }

        public static Language TheLanguage
        {
            get
            {

                if (CultureInfo.CurrentCulture == new CultureInfo("fa-IR"))
                {

                    return Language.FA;
                }
                else
                {
                    return Language.EN;
                }
            }
            set

            {
                if (Language.FA == value)
                {
                    App.ChangeLanguageTo("fa-IR");
                }
                else
                {
                    App.ChangeLanguageTo("en-US");

                }

            }
        }
        public static FlowDirection FlowDirection
        {
            get
            {
                if (TheLanguage == Language.FA)
                {

                    return FlowDirection.RightToLeft;
                }
                else
                {
                    return FlowDirection.LeftToRight;
                }
            }
        }
    }

}
