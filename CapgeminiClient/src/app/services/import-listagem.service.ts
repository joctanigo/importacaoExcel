import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ImportListagemService {

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  constructor(
    private _httpClient: HttpClient
  ) { }
  private url = "https://localhost:44334/api/";

  getAllImports(): Observable<any[]> {
    return this._httpClient.get<any[]>(this.url + "loteimportacaos");
  }
  getAllProdutosById(id): Observable<any[]> {
    return this._httpClient.get<any[]>(this.url + "produtos/getallimports/" + id);
  }

  postFile(fileToUpload: File): Observable<any> {
    const endpoint = this.url+'loteimportacaos/enviararquivo';
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this._httpClient.post(endpoint, formData);
  }
}
