/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, throwError } from 'rxjs';
const username = 'admin';
const password = 'V0c4lc0m';
const base64Credential = btoa(username + ':' + password);

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  public headers: HttpHeaders;
  constructor(private http: HttpClient) {
    const username = 'admin';
    const password = 'V0c4lc0m';
    const base64Credential = btoa(username + ':' + password);
    this.headers = new HttpHeaders({
      'Authorization': 'Basic ' + base64Credential
    });
  }



    APIUrl = "https://call-srv-app.burujinsurance.com/ticketing-api/api/v1";
    //APIUrl = "https://localhost:7012/api/v1";
    AuthUrl = this.APIUrl + "/auth/login";
    ClientInfoUrl = this.APIUrl + "/clientinfo";
    UserUrl = this.APIUrl + "/hdusers";
    UserByNameUrl = this.APIUrl + this.UserUrl + "/byname";
    LogUrl = this.APIUrl + '/log';
    AttachmentUrl = this.APIUrl + "/hdFileattachments";
    CompanyUrl = this.APIUrl + "/hdcompanies";
    ChannelUrl = this.APIUrl + "/hdchannels";
    DepartmentUrl = this.APIUrl + "/hddepartments";
    CategoryUrl = this.APIUrl + "/hdcategories";
    SubCategoryUrl = this.APIUrl + "/hdsubcategories";
    TicketUrl = this.APIUrl + "/hdtickets";
    SubTicketUrl = this.APIUrl + "/ticket";
    CommentUrl = this.APIUrl + "/hdcomments";
    StatusUrl = this.APIUrl + "/hdstatus";
    PriorityUrl = this.APIUrl + "/hdlevels";
    RequestUrl = this.APIUrl + "/hdrequests";
    EscalationUrl = this.APIUrl + "/hdescalation";
    EscalationProfilesUrl = this.APIUrl + "/escalationProfile";
    EscalationLevelsUrl = this.APIUrl + "/escalationLevels";
    EscalationMappingsUrl = this.APIUrl + "/escalationMappings";
    FileUrl = this.APIUrl + "/hdfileattachments";
    ReportUrl = this.APIUrl + "/reports";
    SmtpSettingUrl = this.APIUrl + "/smtp";
    EscalationTimerUrl = this.APIUrl + "/hdescalationtimers";
    LicenseUrl = this.APIUrl + "/licensedetails";
    ProxyUrl = this.APIUrl + "/Proxy/GetPolData";
    ProxyClaimUrl = this.APIUrl + "/Proxy/GetClaimsData";
  countries: any[] = [
    { name: 'United Arab Emirates' },
    { name: 'Afghanistan' },
    { name: 'Albania' },
    { name: 'Algeria' },
    { name: 'Andorra' },
    { name: 'Angola' },
    { name: 'Antigua and Barbuda' },
    { name: 'Argentina' },
    { name: 'Armenia' },
    { name: 'Australia' },
    { name: 'Austria' },
    { name: 'Azerbaijan' },
    { name: 'Bahamas' },
    { name: 'Bahrain' },
    { name: 'Bangladesh' },
    { name: 'Barbados' },
    { name: 'Belarus' },
    { name: 'Belgium' },
    { name: 'Belize' },
    { name: 'Benin' },
    { name: 'Bhutan' },
    { name: 'Bolivia' },
    { name: 'Bosnia and Herzegovina' },
    { name: 'Botswana' },
    { name: 'Brazil' },
    { name: 'Brunei' },
    { name: 'Bulgaria' },
    { name: 'Burkina Faso' },
    { name: 'Burundi' },
    { name: 'Cabo Verde' },
    { name: 'Cambodia' },
    { name: 'Cameroon' },
    { name: 'Canada' },
    { name: 'Central African Republic' },
    { name: 'Chad' },
    { name: 'Chile' },
    { name: 'China' },
    { name: 'Colombia' },
    { name: 'Comoros' },
    { name: 'Congo' },
    { name: 'Costa Rica' },
    { name: 'Croatia' },
    { name: 'Cuba' },
    { name: 'Cyprus' },
    { name: 'Czech Republic' },
    { name: 'Democratic Republic of the Congo' },
    { name: 'Denmark' },
    { name: 'Djibouti' },
    { name: 'Dominica' },
    { name: 'Dominican Republic' },
    { name: 'East Timor' },
    { name: 'Ecuador' },
    { name: 'Egypt' },
    { name: 'El Salvador' },
    { name: 'Equatorial Guinea' },
    { name: 'Eritrea' },
    { name: 'Estonia' },
    { name: 'Eswatini' },
    { name: 'Ethiopia' },
    { name: 'Fiji' },
    { name: 'Finland' },
    { name: 'France' },
    { name: 'Gabon' },
    { name: 'Gambia' },
    { name: 'Georgia' },
    { name: 'Germany' },
    { name: 'Ghana' },
    { name: 'Greece' },
    { name: 'Grenada' },
    { name: 'Guatemala' },
    { name: 'Guinea' },
    { name: 'Guinea-Bissau' },
    { name: 'Guyana' },
    { name: 'Haiti' },
    { name: 'Honduras' },
    { name: 'Hungary' },
    { name: 'Iceland' },
    { name: 'India' },
    { name: 'Indonesia' },
    { name: 'Iran' },
    { name: 'Iraq' },
    { name: 'Ireland' },
    { name: 'Israel' },
    { name: 'Italy' },
    { name: 'Jamaica' },
    { name: 'Japan' },
    { name: 'Jordan' },
    { name: 'Kazakhstan' },
    { name: 'Kenya' },
    { name: 'Kiribati' },
    { name: 'Korea, North' },
    { name: 'Korea, South' },
    { name: 'Kosovo' },
    { name: 'Kuwait' },
    { name: 'Kyrgyzstan' },
    { name: 'Laos' },
    { name: 'Latvia' },
    { name: 'Lebanon' },
    { name: 'Lesotho' },
    { name: 'Liberia' },
    { name: 'Libya' },
    { name: 'Liechtenstein' },
    { name: 'Lithuania' },
    { name: 'Luxembourg' },
    { name: 'Madagascar' },
    { name: 'Malawi' },
    { name: 'Malaysia' },
    { name: 'Maldives' },
    { name: 'Mali' },
    { name: 'Malta' },
    { name: 'Marshall Islands' },
    { name: 'Mauritania' },
    { name: 'Mauritius' },
    { name: 'Mexico' },
    { name: 'Micronesia' },
    { name: 'Moldova' },
    { name: 'Monaco' },
    { name: 'Mongolia' },
    { name: 'Montenegro' },
    { name: 'Morocco' },
    { name: 'Mozambique' },
    { name: 'Myanmar' },
    { name: 'Namibia' },
    { name: 'Nauru' },
    { name: 'Nepal' },
    { name: 'Netherlands' },
    { name: 'New Zealand' },
    { name: 'Nicaragua' },
    { name: 'Niger' },
    { name: 'Nigeria' },
    { name: 'North Macedonia' },
    { name: 'Norway' },
    { name: 'Oman' },
    { name: 'Pakistan' },
    { name: 'Palau' },
    { name: 'Palestine' },
    { name: 'Panama' },
    { name: 'Papua New Guinea' },
    { name: 'Paraguay' },
    { name: 'Peru' },
    { name: 'Philippines' },
    { name: 'Poland' },
    { name: 'Portugal' },
    { name: 'Qatar' },
    { name: 'Romania' },
    { name: 'Russia' },
    { name: 'Rwanda' },
    { name: 'Saint Kitts and Nevis' },
    { name: 'Saint Lucia' },
    { name: 'Saint Vincent and the Grenadines' },
    { name: 'Samoa' },
    { name: 'San Marino' },
    { name: 'Sao Tome and Principe' },
    { name: 'Saudi Arabia' },
    { name: 'Senegal' },
    { name: 'Serbia' },
    { name: 'Seychelles' },
    { name: 'Sierra Leone' },
    { name: 'Singapore' },
    { name: 'Slovakia' },
    { name: 'Slovenia' },
    { name: 'Solomon Islands' },
    { name: 'Somalia' },
    { name: 'South Africa' },
    { name: 'South Sudan' },
    { name: 'Spain' },
    { name: 'Sri Lanka' },
    { name: 'Sudan' },
    { name: 'Suriname' },
    { name: 'Sweden' },
    { name: 'Switzerland' },
    { name: 'Syria' },
    { name: 'Taiwan' },
    { name: 'Tajikistan' },
    { name: 'Tanzania' },
    { name: 'Thailand' },
    { name: 'Togo' },
    { name: 'Tonga' },
    { name: 'Trinidad and Tobago' },
    { name: 'Tunisia' },
    { name: 'Turkey' },
    { name: 'Turkmenistan' },
    { name: 'Tuvalu' },
    { name: 'Uganda' },
    { name: 'Ukraine' },
    { name: 'United Kingdom' },
    { name: 'United States' },
    { name: 'Uruguay' },
    { name: 'Uzbekistan' },
    { name: 'Vanuatu' },
    { name: 'Vatican City' },
    { name: 'Venezuela' },
    { name: 'Vietnam' },
    { name: 'Yemen' },
    { name: 'Zambia' },
    { name: 'Zimbabwe' },
  ];

  regions: any[] = [
    { name: 'Middle East' },
    { name: 'Africa' },
    { name: 'Asia' },
    { name: 'Europe' },
    { name: 'North America' },
    { name: 'Oceania' },
    { name: 'South America' },
    { name: 'Central America' },
    { name: 'Caribbean' },
    { name: 'Central Asia' },
    { name: 'Eastern Europe' },
    { name: 'Northern Europe' },
    { name: 'Southern Europe' },
    { name: 'Western Europe' },
  ];
}
