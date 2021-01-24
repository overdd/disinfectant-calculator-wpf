using System;

namespace WpfApplication1
{
    public static class ValidationMethods
    {
        public static void ValidateUserCredentials(string login, string password)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                throw new Exception("Ошибка ввода логина или пароля");
            }
            
            if (login.Length > 10 || password.Length > 10)
            {
                throw new Exception("Длина логина или пароля должна быть меньше 10 символов");
            }
        }

        public static void ValidatePositionForm(
            string objectNameDez,
            string objectCountDez,
            string objectArea,
            string processType,
            string processesPerMonth,
            string dezinfectionThing,
            string workConcentration,
            string rashodRastvora,
            string kolichestvoRashodaOdnokrat,
            string dezType)
        {
            int stub;
            float stubf;
            if (!Int32.TryParse(objectCountDez, out stub))
            {
                throw new Exception("Ошибка ввода количества объектов. Ожидается целое число.");
            }

            if (!Single.TryParse(objectArea, out stubf))
            {
                throw new Exception("Ошибка ввода площади. Возможно используется \",\" вместо \".\" для отделения дробной части");
            }

            if (!(processType == "Т" || processType == "Г"))
            {
                throw new Exception("Вид обработки имеет вид Т или Г кирилицей");
            }

            if (!Int32.TryParse(processesPerMonth, out stub))
            {
                throw new Exception("Кратность обработок в месяц. Ожидается целое число.");
            }
            
            if (!(dezType == "Т" || dezType == "Ж"))
            {
                throw new Exception("Вид дез. средства имеет вид Т или Ж кирилицей");
            }
            
            if (!Single.TryParse(workConcentration, out stubf))
            {
                throw new Exception("Ошибка ввода рабочей концентрации (%) или кол-во таблеток. Возможно используется \",\" вместо \".\" для отделения дробной части");
            }
            
            if (!Single.TryParse(rashodRastvora, out stubf))
            {
                throw new Exception("Ошибка ввода расхода рабочего раствора на один кв.м.. Возможно используется \",\" вместо \".\" для отделения дробной части");
            }
        }

        public static void ValidateCellEdit(int index, string value, in Record rowToValidate)
        {
            int stub;
            float stubf;
            switch (index)
            {
                case 2:
                {
                    if (!Int32.TryParse(value, out stub))
                    {
                        throw new Exception("Ошибка ввода количества объектов. Ожидается целое число.");
                    }
                    
                    rowToValidate.ObjectCountDez = value;
                }
                break;

                case 3:
                {
                    if (!Single.TryParse(value, out stubf))
                    {
                        throw new Exception("Ошибка ввода площади. Возможно используется \",\" вместо \".\" для отделения дробной части");
                    }
                    
                    rowToValidate.ObjectArea = value;
                }
                break;
                
                case 4:
                {
                    if (!(value == "Т" || value == "Г"))
                    {
                        throw new Exception("Вид обработки имеет вид Т или Г кирилицей");
                    }

                    rowToValidate.ProcessType = value;
                }
                break;
                
                case 5:
                {
                    if (!(value == "Т" || value == "Ж"))
                    {
                        throw new Exception("Вид обработки имеет вид Т или Г кирилицей");
                    }

                    rowToValidate.DezType = value;
                }
                break;
                
                case 6:
                {
                    if (!Int32.TryParse(value, out stub))
                    {
                        throw new Exception("Кратность обработок в месяц. Ожидается целое число.");
                    }

                    rowToValidate.ProcessesPerMonth = stub;
                }
                break;
                
                case 8:
                {
                    if (!Single.TryParse(value, out stubf))
                    {
                        throw new Exception("Ошибка ввода рабочей концентрации (%) или кол-во таблеток. Возможно используется \",\" вместо \".\" для отделения дробной части");
                    }

                    rowToValidate.WorkConcentration = stubf;
                }
                break;
                
                case 9:
                {
                    if (!Single.TryParse(value, out stubf))
                    {
                        throw new Exception("Ошибка ввода расхода рабочего раствора на один кв.м.. Возможно используется \",\" вместо \".\" для отделения дробной части");
                    }

                    rowToValidate.RashodRastvora = value;
                }
                break;
                
                case 10:
                {
                    if (!Single.TryParse(value, out stubf))
                    {
                        throw new Exception("Ошибка ввода расхода рабочего раствора однократной обработки. Возможно используется \",\" вместо \".\" для отделения дробной части");
                    }

                    rowToValidate.KolichestvoRashodaOdnokrat = stubf;
                }
                break;
            }
        }
    }
}