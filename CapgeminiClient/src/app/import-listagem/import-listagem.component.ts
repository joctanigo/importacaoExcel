import { ImportListagemService } from './../services/import-listagem.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-import-listagem',
  templateUrl: './import-listagem.component.html',
  styleUrls: ['./import-listagem.component.css']
})
export class ImportListagemComponent implements OnInit {

  fileToUpload: File = null;

  constructor(
    private _importacaoListagemService: ImportListagemService
  ) { }
  errosImportacao:Array<any>=[]
  resLotes: Array<any> = [];
  ngOnInit() {
    this.getLotes();

  }

  public getLotes() {
    this._importacaoListagemService.getAllImports().subscribe(res => {
      this.resLotes = res;
    })

  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

  uploadFileToActivity() {
    this._importacaoListagemService.postFile(this.fileToUpload).subscribe(res => {
      this.getLotes();
    }, error => {
     this.errosImportacao = error.error;

    });
  }
}
