import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Book, PagedResult } from '../../models/book';
import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.css'
})
export class BookListComponent implements OnInit {
  private bookService = inject(BookService);
  private router = inject(Router);

  result: PagedResult<Book> = {
    items: [],
    page: 1,
    pageSize: 5,
    totalCount: 0,
    totalPages: 0
  };

  search = '';
  loading = false;
  error = '';

  ngOnInit(): void {
    this.loadBooks(1);
  }

  loadBooks(page: number): void {
    this.loading = true;
    this.error = '';

    this.bookService.getBooks(page, this.result.pageSize, this.search).subscribe({
      next: (response) => {
        this.result = response;
        this.loading = false;
      },
      error: () => {
        this.error = 'Nem sikerült betölteni a könyveket.';
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.loadBooks(1);
  }

  prevPage(): void {
    if (this.result.page > 1) {
      this.loadBooks(this.result.page - 1);
    }
  }

  nextPage(): void {
    if (this.result.page < this.result.totalPages) {
      this.loadBooks(this.result.page + 1);
    }
  }

  editBook(id?: string): void {
    if (!id) return;
    this.router.navigate(['/books/edit', id]);
  }

  deleteBook(id?: string): void {
    if (!id) return;

    const confirmed = confirm('Biztosan törölni szeretnéd ezt a könyvet?');
    if (!confirmed) return;

    this.bookService.deleteBook(id).subscribe({
      next: () => this.loadBooks(this.result.page),
      error: () => this.error = 'A törlés nem sikerült.'
    });
  }
}