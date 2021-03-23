using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PedidosDeCompra.Models;

namespace PedidosDeCompra.Controllers
{
    public class PedidosController : Controller
    {
        private readonly PedidosDeCompraContext _context;

        public PedidosController(PedidosDeCompraContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var pedidosDeCompraContext = _context.Pedidos.Include(p => p.Cliente);
            return View(await pedidosDeCompraContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(pedido=>pedido.Produtos)
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nome");
            List<Produto> produtos = _context.Produtos.Where(p => p.QuantidadeDisponivel > 0).ToList();
            ViewData["ProdutoId"] = new SelectList(produtos, "ProdutoId", "Nome");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PedidoId,ValorTotal,DataPedido,ClienteId")] Pedido pedido, List<int> ProdutoId)
        {
            if (ModelState.IsValid)
            {
                //colocar a data do pedido
                pedido.DataPedido = DateTime.Now.ToString();
                _context.Add(pedido);
                //como está indo no banco, teremos o Id do pedido nessa var pedido
                await _context.SaveChangesAsync();
                //Passaremos por todos os itens selecionados na tela, buscando esses
                // objetos no banco de dados
                foreach(var id in ProdutoId){
                    Produto produto = _context.Produtos.Find(id);
                    produto.PedidoId = pedido.PedidoId;
                    //decrementando a quantidade disponível, já q está sendo add a um pedido
                    produto.QuantidadeDisponivel --;
                    //somando o valor total do pedido
                    pedido.ValorTotal = pedido.ValorTotal + produto.Valor;
                    //o id do pedido foi add ao produto, precisamos registrar essa alteração 
                    // no banco de dados
                    _context.Update(produto);
                    //O pedido foi alterado o valor total, logo, passaremos esta informação para o banco
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nome", pedido.ClienteId);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nome", pedido.ClienteId);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PedidoId,ValorTotal,DataPedido,ClienteId")] Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.PedidoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nome", pedido.ClienteId);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.PedidoId == id);
        }
    }
}
