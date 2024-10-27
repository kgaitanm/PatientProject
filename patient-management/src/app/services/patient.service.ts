import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable,of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Patient } from '../models/patient.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private apiUrl = 'https://localhost:7286/api/patient';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): Observable<HttpHeaders> {
    return this.authService.getToken().pipe(
      switchMap(token => {
        const headers = new HttpHeaders({
          Authorization: token ? `Bearer ${token.access_token}` : ''
        });
        return of(headers);
      })
    );
  }

  getPatients(): Observable<Patient[]> {
    return this.getAuthHeaders().pipe(
      switchMap(headers => this.http.get<Patient[]>(this.apiUrl, { headers }))
    );
  }

  getPatient(id: string): Observable<Patient> {
    return this.getAuthHeaders().pipe(
      switchMap(headers => this.http.get<Patient>(`${this.apiUrl}/${id}`, { headers }))
    );
  }

  registerPatient(patient: Patient): Observable<Patient> {
    return this.getAuthHeaders().pipe(
      switchMap(headers => this.http.post<Patient>(this.apiUrl, patient, { headers }))
    );
  }

  updatePatient(patient: Patient): Observable<Patient> {
    return this.getAuthHeaders().pipe(
      switchMap(headers => this.http.put<Patient>(`${this.apiUrl}/${patient.id}`, patient, { headers }))
    );
  }

  deletePatient(id: string): Observable<void> {
    return this.getAuthHeaders().pipe(
      switchMap(headers => this.http.delete<void>(`${this.apiUrl}/${id}`, { headers }))
    );
  }
}