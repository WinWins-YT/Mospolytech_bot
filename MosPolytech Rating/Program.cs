using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Telegram.Bot;
using System.Globalization;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.ReplyMarkups;

namespace MosPolytech_Rating
{
    class Program
    {
        TelegramBotClient bot = new TelegramBotClient("TELEGRAM_BOT_TOKEN_HERE");
        Regex regex = new Regex(@"\d{3}-\d{3}-\d{3} \d{2}");
        List<User> users = new List<User>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Program().Run();
        }

        private void Run()
        {
            bot.StartReceiving();
            bot.OnMessage += Bot_OnMessage;
            while (true)
            {

            }
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                bool exists = false;
                foreach (User user in users) if (user.ChatId == e.Message.Chat.Id) exists = true;
                if (!exists) users.Add(new User() { ChatId = e.Message.Chat.Id });
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Введите свой СНИЛС с командой /snils (Пример: /snils 123-123-123 90)", replyMarkup: new ReplyKeyboardRemove());
            }
            else if (e.Message.Text.Contains("/snils"))
            {
                User user = null;
                foreach (User _user in users) if (_user.ChatId == e.Message.Chat.Id) user = _user;
                if (user == null) { bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте команду /start", replyMarkup: new ReplyKeyboardRemove()); return; }
                else
                {
                    if (e.Message.Text.Split(' ').Length != 3)
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "Команда /snils использована неверно", replyMarkup: new ReplyKeyboardRemove());
                        return;
                    }
                    if (regex.IsMatch(e.Message.Text.Split(' ')[1] + " " + e.Message.Text.Split(' ')[2]))
                    {
                        user.SNILS = e.Message.Text.Split(' ')[1] + " " + e.Message.Text.Split(' ')[2];
                        var markup = new ReplyKeyboardMarkup();
                        markup.Keyboard = new KeyboardButton[][]
                        {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/loc Москва")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/loc Рязань")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/loc Коломна")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/loc Чебоксары")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/loc Электросталь")
                            },
                        };
                        markup.OneTimeKeyboard = true;
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите филиал из списка ниже", replyMarkup: markup);
                    }
                    else bot.SendTextMessageAsync(e.Message.Chat.Id, "СНИЛС введен неверно", replyMarkup: new ReplyKeyboardRemove());
                }
            }
            else if (e.Message.Text.Contains("/loc"))
            {
                User user = null;
                foreach (User _user in users) if (_user.ChatId == e.Message.Chat.Id) user = _user;
                if (user == null) { bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте команду /start", replyMarkup: new ReplyKeyboardRemove()); return; }
                else
                {
                    if (e.Message.Text.Split(' ').Length != 2)
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "Команда /loc использована неверно");
                        return;
                    }
                    string temp = e.Message.Text.Split(' ')[1];
                    if (temp == "Москва") user.Location = User.Locations.Moscow;
                    else if (temp == "Рязань") user.Location = User.Locations.Ryzen;
                    else if (temp == "Коломна") user.Location = User.Locations.Kolomna;
                    else if (temp == "Чебоксары") user.Location = User.Locations.Cheboksary;
                    else if (temp == "Электросталь") user.Location = User.Locations.Elektrostal;
                    else { bot.SendTextMessageAsync(e.Message.Chat.Id, "Пожалуйста, используйте клавиатуру"); return; }
                    var markup = new ReplyKeyboardMarkup();
                    markup.Keyboard = new KeyboardButton[][]
                    {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/fac 09.03.01.01 Веб-технологии")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/fac 09.03.01.04 Киберфизические системы")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/fac 09.03.02.04 Цифровая трансформация")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/fac 09.03.03.02 Большие и открытые данные")
                            },
                    };
                    markup.OneTimeKeyboard = true;
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите направление из списка ниже", replyMarkup: markup);
                }
            }
            else if (e.Message.Text.Contains("/fac"))
            {
                User user = null;
                foreach (User _user in users) if (_user.ChatId == e.Message.Chat.Id) user = _user;
                if (user == null) { bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте команду /start", replyMarkup: new ReplyKeyboardRemove()); return; }
                else
                {
                    if (e.Message.Text.Split(' ').Length < 3)
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "Команда /fac использована неверно");
                        return;
                    }
                    string temp = e.Message.Text;
                    if (temp == "/fac 09.03.01.01 Веб-технологии") user.Facultet = User.Facultets.Web;
                    else if (temp == "/fac 09.03.01.04 Киберфизические системы") user.Facultet = User.Facultets.CyberPhysic;
                    else if (temp == "/fac 09.03.02.04 Цифровая трансформация") user.Facultet = User.Facultets.Digital;
                    else if (temp == "/fac 09.03.03.02 Большие и открытые данные") user.Facultet = User.Facultets.Data;
                    else { bot.SendTextMessageAsync(e.Message.Chat.Id, "Пожалуйста, используйте клавиатуру"); return; }
                    var markup = new ReplyKeyboardMarkup();
                    markup.Keyboard = new KeyboardButton[][]
                    {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/form Очная")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/form Очно-заочная")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/form Заочная")
                            },
                    };
                    markup.OneTimeKeyboard = true;
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите форму обучения из списка ниже", replyMarkup: markup);
                }
            }
            else if (e.Message.Text.Contains("/form"))
            {
                User user = null;
                foreach (User _user in users) if (_user.ChatId == e.Message.Chat.Id) user = _user;
                if (user == null) { bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте команду /start", replyMarkup: new ReplyKeyboardRemove()); return; }
                else
                {
                    if (e.Message.Text.Split(' ').Length != 2)
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "Команда /form использована неверно");
                        return;
                    }
                    string temp = e.Message.Text.Split(' ')[1];
                    if (temp == "Очная") user.FormStudy = User.FormStudies.Present;
                    else if (temp == "Очно-заочная") user.FormStudy = User.FormStudies.SemiPresent;
                    else if (temp == "Заочная") user.FormStudy = User.FormStudies.NotPresent;
                    else { bot.SendTextMessageAsync(e.Message.Chat.Id, "Пожалуйста, используйте клавиатуру"); return; }
                    var markup = new ReplyKeyboardMarkup();
                    markup.Keyboard = new KeyboardButton[][]
                    {
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/fin Бюджетная")
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton("/fin Платная")
                            },
                    };
                    markup.OneTimeKeyboard = true;
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите основу обучения из списка ниже", replyMarkup: markup);
                }
            }
            else if (e.Message.Text.Contains("/fin"))
            {
                User user = null;
                foreach (User _user in users) if (_user.ChatId == e.Message.Chat.Id) user = _user;
                if (user == null) { bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте команду /start", replyMarkup: new ReplyKeyboardRemove()); return; }
                else
                {
                    if (e.Message.Text.Split(' ').Length != 2)
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "Команда /fin использована неверно");
                        return;
                    }
                    string temp = e.Message.Text.Split(' ')[1];
                    if (temp == "Бюджетная") user.FinStudy = User.FinStudies.Free;
                    else if (temp == "Платная") user.FinStudy = User.FinStudies.Paid;
                    else { bot.SendTextMessageAsync(e.Message.Chat.Id, "Пожалуйста, используйте клавиатуру"); return; }
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот и все. Бот запомнил информацию и может выдать результаты по команде /show. Если бот вас послал, то пройдите регистрацию еще раз по команде /start. Спасибо за использование\n(С) WinWins", replyMarkup: new ReplyKeyboardRemove()); return;
                }
            }
            else
            {
                if (e.Message.Text == "/show")
                {
                    try
                    {
                        User user = null;
                        foreach (User _user in users) if (_user.ChatId == e.Message.Chat.Id) user = _user;
                        if (user == null) { bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте комманду /start", replyMarkup: new ReplyKeyboardRemove()); return; }
                        else if (user != null && (user.ChatId == 0 || user.Facultet == null || user.FinStudy == null || user.FormStudy == null || user.Location == null || user.SNILS == "")) { bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте команду /start и пройдите сбор информации до конца", replyMarkup: new ReplyKeyboardRemove()); return; }
                        Console.WriteLine("New request");
                        bool sent = false;
                        WebRequest web = WebRequest.Create("https://raitinglistpk.mospolytech.ru/rating_list_ajax.php");
                        web.Method = "POST";
                        web.ContentType = "application/x-www-form-urlencoded";
                        string postData = "select1=" + Uri.EscapeDataString(user.LocationStr) +
                            "&specCode=" + Uri.EscapeDataString(user.FacultetString) +
                            "&eduForm=" + Uri.EscapeDataString(user.FormStudyString) +
                            "&eduFin=" + Uri.EscapeDataString(user.FinStudyString);
                        var data = Encoding.UTF8.GetBytes(postData);
                        web.ContentLength = data.Length;
                        using (var stream = web.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        StreamReader sr = new StreamReader(web.GetResponse().GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                        string response = sr.ReadToEnd();
                        sr.Dispose();
                        DateTime lastUpdated = new DateTime(2021, 7, 29, 18, 0, 0);
                        for (int i = 0; i < response.Length - 16; i++)
                        {
                            if (DateTime.TryParseExact(response.Substring(i, 16), "dd.MM.yyyy HH:mm", new CultureInfo("ru-ru"), DateTimeStyles.None, out lastUpdated))
                                break;
                        }
                        List<string> rows = new List<string>();
                        string row = "";
                        for (int i = 0; i < response.Length - 3; i++)
                        {
                            row += response[i];
                            if (response.Substring(i, 3) == "<td")
                            {
                                rows.Add(row);
                                row = "<";
                            }
                        }
                        if (row != "<") rows.Add(row);
                        string snils = user.SNILS;
                        Console.WriteLine($"SNILS: {snils}");
                        for (int i = 0; i < rows.Count; i++)
                        {
                            if (rows[i].Contains(snils))
                            {
                                string temp = rows[i - 1];
                                string konkurs = "";
                                string obsh = "";
                                string all = "";
                                string budg = "";
                                for (int j = 0; j < temp.Length - 20; j++)
                                {
                                    if (temp.Substring(j, 20) == "<font color=\"black\">")
                                    {
                                        j += 20;
                                        konkurs = "";
                                        for (int m = j; m < temp.Length; m++)
                                        {
                                            if (temp[m] == '<') break;
                                            else konkurs += temp[m];
                                        }
                                    }
                                }
                                temp = rows[i - 2];
                                for (int j = 0; j < temp.Length - 9; j++)
                                {
                                    if (temp.Substring(j, 9) == "<b>&nbsp;")
                                    {
                                        j += 9;
                                        obsh = "";
                                        for (int m = j; m < temp.Length; m++)
                                        {
                                            if (temp[m] == '&') break;
                                            else obsh += temp[m];
                                        }
                                    }
                                }
                                temp = rows[rows.Count - 1];
                                List<string> lis = new List<string>();
                                string li = "";
                                for (int j = 0; j < temp.Length - 4; j++)
                                {
                                    li += temp[j];
                                    if (temp.Substring(j, 4) == "<li>")
                                    {
                                        lis.Add(li);
                                        li = "<";
                                    }
                                }
                                if (li != "<") lis.Add(li);
                                foreach (string listr in lis)
                                {
                                    if (listr.Contains("УЧАСТВУЮЩИЕ В КОНКУРСЕ"))
                                    {
                                        string temp1 = listr.Split(':')[1];
                                        for (int m = 1; m < temp1.Length; m++)
                                        {
                                            if (temp1[m] == '<') break;
                                            else all += temp1[m];
                                        }
                                    }
                                    else if (listr.Contains("БЮДЖЕТНЫХ МЕСТ ПО НАПРАВЛЕНИЮ"))
                                    {
                                        string temp1 = listr.Split(':')[1];
                                        for (int m = 21; m < temp1.Length; m++)
                                        {
                                            if (temp1[m] == '<') break;
                                            else budg += temp1[m];
                                        }
                                    }
                                }
                                string reply = "Ваше место в рейтинге: " + konkurs + "\n" +
                                    "Ваше место в общем рейтинге: " + obsh + "\n" +
                                    "Всего заявлений: " + all + "\n" +
                                    "Всего бюджетных мест: " + budg + "\n" +
                                    "Последнее обновление было " + lastUpdated.ToString("dd.MM.yyyy HH:mm");
                                bot.SendTextMessageAsync(e.Message.Chat.Id, reply, replyMarkup: new ReplyKeyboardRemove());
                                Console.WriteLine($"Place: {konkurs}");
                                sent = true;
                            }
                        }
                        if (!sent) bot.SendTextMessageAsync(e.Message.Chat.Id, "Вас нет в рейтинговых списках", replyMarkup: new ReplyKeyboardRemove());
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "Возникла ошибка\nКод ошибки: " + ex.Message, replyMarkup: new ReplyKeyboardRemove()); return;
                    }
                }
                else
                {
                    bool exists = false;
                    foreach (User user in users) if (user.ChatId == e.Message.Chat.Id) exists = true;
                    if (!exists) bot.SendTextMessageAsync(e.Message.Chat.Id, "Используйте команду /start", replyMarkup: new ReplyKeyboardRemove());
                    else bot.SendTextMessageAsync(e.Message.Chat.Id, "Команда задана неверно");
                }
            }
        }
    }
}
