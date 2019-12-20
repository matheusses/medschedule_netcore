import { Datetimeformat } from "../_helps/datetimeformat.pipe";

export interface Meeting {
  id: number;
  beginMeeting: Date;
  endMeeting: Date;
  birth: Date;
  observation: string        
  patientName: string; 
}
