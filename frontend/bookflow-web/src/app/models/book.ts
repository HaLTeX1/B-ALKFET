export interface Book {
  id?: string;
  title: string;
  author: string;
  genre: string;
  publishedYear: number;
  availableCopies: number;
  createdAtUtc?: string;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}