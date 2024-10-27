import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenUrl = 'https://localhost:7286/token';
  private clientId = 'PatientManagementClient';
  private clientSecret = 'UGF0aWVudE1hbmFnZW1lbnRDbGllbnQ=';
  private token: string | null = null;

  constructor(private http: HttpClient) {}

  getToken(): Observable<any> {
    

    if (this.token) {
     
      return of({ access_token: this.token });
    }

   
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Basic ' + btoa(`${this.clientId}:${this.clientSecret}`)
    });

    return this.http.post<any>(this.tokenUrl, { clientId: this.clientId, clientSecret: this.clientSecret }, { headers }).pipe(
      tap(response => {
        if (response && response.access_token) {
          this.token = response.access_token;
        } else {
          console.error(' Invalid token response:', response);
        }
      }),
      catchError(error => {
        console.error(' Error while getting token:', error);
        return of(null); 
      })
    );
  }
}
