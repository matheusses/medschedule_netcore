import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Meeting } from '../_models/Meeting';


@Injectable({
  providedIn: 'root'
})

export class MeetingService {
  baseURL = "https://localhost:44364/api/meeting";
  constructor(private http: HttpClient) { }

  getAllMeeting(): Observable<Meeting[]> {
    return this.http.get<Meeting[]>(this.baseURL+"/getall");
  }

  postMeeting(meeting: Meeting){
    return this.http.post(this.baseURL, meeting);
  }
  
  putMeeting(meeting: Meeting) {
    return this.http.put(this.baseURL, meeting);
  }
  deleteMeeting(id: number) {

    return this.http.delete(this.baseURL+'/delete/'+id);
  }




}
