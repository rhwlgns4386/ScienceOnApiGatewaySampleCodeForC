using Newtonsoft.Json.Linq;
using System.Net;
using System;
using System.IO;
using System.Web;
using AES256;
using System.Xml;

namespace TokenSample
{
    class TokenSample
    {
        /**============================ 사용자 정보 입력 ==========================================*/
                //맥주소 입력해주세요.
        public static string MAC_address = "Your Mac Address";
        //발급받은 client_id를 입력해주세요.
        public static string clientID = "Your clientId";
        //API Gateway로부터 발급받은 인증키를 입력해주세요.
        public static string key = "Your key";

        //refresh Token을 입력해주세요.(Access Token 재발급시 입력)
        public static string refreshToken = "Your RefreshToken";
        //accessToken을 입력해주세요(데이터요청시 이용)
        public static string accessToken = "Your AccessToken";
        /**====================================== ==========================================*/


        public static void Main_a()
        {
        /**
         * 최초 token요청을 합니다.
         * RefreshToken과 AccessToken을 발급 받습니다.
         */
            AES256Util aes256Util = new AES256Util();
            string tokenResponse = createToken(aes256Util);
            Console.WriteLine(tokenResponse);

        /** 데이터를 요청하고 그에 맞는 데이터를 받습니다.
        * 하단의 코드는 예제코드이며 자세한 내용은 ScienceOn ApiGateWay를 참조해주세요.
        */
            string query = HttpUtility.UrlEncode("{\"BI\":\"코로나\"}");
            string target_URL = "https://apigateway.kisti.re.kr/openapicall.do?" +
            "client_id=" + clientID + "&token=" + accessToken + "&version=1.0" + "&action=search" +
            "&target=ARTI" + "&searchQuery=" + query;

            string response = getResponse(target_URL);
            Console.WriteLine(response);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response);

        /**
         * AccessToken만료시 AccessToken를 재발급합니다.
         * AccessToken요청시 E4106번이 떨어지면 RefreshToken 만료된 것 입니다.
         */
            if (!xml["MetaData"]["resultSummary"]["statusCode"].InnerText.Equals("200"))
            {
                if (response.Contains("E4103"))
                {
                    Console.WriteLine("AccessToken이 만료 되었습니다.");
                    tokenResponse = getAccessToken();
                    if (tokenResponse.Contains("E4106"))
                    {
                        Console.WriteLine("RefreshToken이 만료 되었습니다.");
                        createToken(aes256Util);
                    }
                }

            }

        }

    /**
     * 1) 최초 토큰발급 요청인 경우, RefreshToken 값이 만료(2주 기한)되어 신규로 AccessToken, RefreshToken 둘 다 전체 토큰발급이 필요한 경우
     * 2) API Gateway 신청시 제출한 맥주소 값, 발급받은 클라이어트 ID 값이 필요함
     * 3) 정상적으로 토큰발급이 완료되면, AccessToken, RefreshToken 값을 저장한 이 후에 이 값을 사용해야 함
     */
        public static string createToken(AES256Util aes256Util)
        {
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");

            var json = new JObject();
            json.Add("datetime", date);
            json.Add("mac_address", MAC_address);
            
            string encrypted_txt = HttpUtility.UrlEncode(aes256Util.encrypt(json.ToString(), key));

            string target_URL = "https://apigateway.kisti.re.kr/tokenrequest.do?accounts=" + encrypted_txt +
            "&client_id=" + clientID;


            JObject responseJson = JObject.Parse(getResponse(target_URL));
            refreshToken = responseJson["refresh_token"].ToString();
            accessToken = responseJson["access_token"].ToString();
            Console.WriteLine("access_token : " + accessToken);
            Console.WriteLine("refresh_token : " + refreshToken);
            return responseJson.ToString();
        }

    /**
     * 1) AccessToken 값이 만료(2시간 기한)되어 신규로 AccessToken 발급이 필요한 경우
     * 2) 최초 토큰발급 또는 전체 토큰발급 받을 때 받은 RefreshToken 값을 가지고 URL 호출함
     * 3) 정상적으로 토큰발급이 완료되면, AccessToken 값을 저장한 이 후에 이 값을 사용해야 함
     * 4) 토큰발급이 안되면 신규로 전체 토큰발급 진행을 해야 함
     */
        public static string getAccessToken()
        {
            string target_URL = "https://apigateway.kisti.re.kr/tokenrequest.do?client_id=" + clientID
            + "&refreshToken=" + refreshToken;

            JObject responseJson = JObject.Parse(getResponse(target_URL));
            if (!responseJson.ToString().Contains("errorCode")) {
                accessToken = responseJson["access_token"].ToString();
            }
            return responseJson.ToString();
        }

    /**
     * @brief 서버로 request요청을 보내고 그에 맞는 response를 받는
     * @return String:요청을 받은 xml or json값
     * @param target_URL:요청을 보낼 url
     */
        public static string getResponse(string target_URL)
        {
            string responseText = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target_URL);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    HttpStatusCode statusCode = response.StatusCode;
                    Console.WriteLine(statusCode);

                    Stream respStream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return responseText;
        }

    }

}