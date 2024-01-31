import { Injectable } from '@angular/core';
import { City } from '../models/city';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const API_BASE_URL: string = 'https://localhost:7122/api/';

@Injectable({
  providedIn: 'root',
})
export class CitiesService {
  cities: City[] = [];

  constructor(private httpClient: HttpClient) {}

  public getCities(): Observable<City[]> {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', 'Bearer mytoken');
    return this.httpClient.get<City[]>(`${API_BASE_URL}v1/cities`, {
      headers: headers,
    });
  }

  public postCity(city: City): Observable<City> {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', 'Bearer mytoken');
    return this.httpClient.post<City>(`${API_BASE_URL}v1/cities`, city, {
      headers: headers,
    });
  }

  public putCity(city: City): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', 'Bearer mytoken');
    return this.httpClient.put<string>(
      `${API_BASE_URL}v1/cities/${city.cityId}`,
      city,
      {
        headers: headers,
      },
    );
  }

  public deleteCity(cityId: string | null): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', 'Bearer mytoken');
    return this.httpClient.delete<string>(
      `${API_BASE_URL}v1/cities/${cityId}`,
      {
        headers: headers,
      },
    );
  }
}
