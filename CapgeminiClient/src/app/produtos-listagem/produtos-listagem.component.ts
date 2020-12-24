import { Component, OnInit, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ImportListagemService } from '../services/import-listagem.service';

@Component({
  selector: 'app-produtos-listagem',
  templateUrl: './produtos-listagem.component.html',
  styleUrls: ['./produtos-listagem.component.css']
})
export class ProdutosListagemComponent implements OnInit {

  protected route: ActivatedRoute;
  constructor(
    protected injector: Injector,
    private _importacaoListagemService: ImportListagemService
  ) {
    this.route = this.injector.get(ActivatedRoute);
  }
  resProdutos: Array<any> = [];
  ngOnInit() {
    this.getLoteProdutos();
  }

  public getLoteProdutos(){
    this._importacaoListagemService.getAllProdutosById(this.route.snapshot.url[0].path).subscribe(res=>{
      this.resProdutos =res;
    })
  }


}
