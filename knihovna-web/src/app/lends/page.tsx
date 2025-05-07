'use client'

import { useState, useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'

interface Lend {
  id: number
  bookId: number
  userId: number
  lended: string
  returned: string | null
  book: {
    name: string
    author: string
  }
  user: {
    name: string
  }
}

interface Book {
  id: number
  name: string
  author: string
  inStock: number
}

interface User {
  id: number
  name: string
}

const schema = yup.object({
  bookId: yup.number().required('Kniha je povinná'),
  userId: yup.number().required('Uživatel je povinný'),
}).required()

type LendFormData = yup.InferType<typeof schema>

export default function LendsPage() {
  const [lends, setLends] = useState<Lend[]>([])
  const [books, setBooks] = useState<Book[]>([])
  const [users, setUsers] = useState<User[]>([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [error, setError] = useState('')

  const { register, handleSubmit, reset, formState: { errors } } = useForm<LendFormData>({
    resolver: yupResolver(schema)
  })

  useEffect(() => {
    fetchLends()
    fetchBooks()
    fetchUsers()
  }, [])

  const fetchLends = async () => {
    try {
      const response = await fetch('/api/lends')
      if (response.ok) {
        const data = await response.json()
        setLends(data)
      } else {
        setError('Chyba při načítání výpůjček')
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Chyba při načítání výpůjček')
    }
  }

  const fetchBooks = async () => {
    try {
      const response = await fetch('/api/books')
      if (response.ok) {
        const data = await response.json()
        setBooks(data)
      } else {
        setError('Chyba při načítání knih')
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Chyba při načítání knih')
    }
  }

  const fetchUsers = async () => {
    try {
      const response = await fetch('/api/users')
      if (response.ok) {
        const data = await response.json()
        setUsers(data)
      } else {
        setError('Chyba při načítání uživatelů')
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Chyba při načítání uživatelů')
    }
  }

  const onSubmit = async (data: LendFormData) => {
    try {
      const response = await fetch('/api/lends', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      })

      if (response.ok) {
        await fetchLends()
        setIsModalOpen(false)
        reset()
      } else {
        const error = await response.json()
        setError(error.message || 'Chyba při vytváření výpůjčky')
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Chyba při vytváření výpůjčky')
    }
  }

  const handleReturn = async (lendId: number) => {
    try {
      const response = await fetch(`/api/lends/${lendId}/return`, {
        method: 'POST',
      })

      if (response.ok) {
        await fetchLends()
      } else {
        const error = await response.json()
        setError(error.message || 'Chyba při vracení knihy')
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Chyba při vracení knihy')
    }
  }

  const handleAdd = () => {
    reset({ bookId: 0, userId: 0 })
    setIsModalOpen(true)
  }

  return (
    <div className="px-4 sm:px-6 lg:px-8">
      <div className="sm:flex sm:items-center">
        <div className="sm:flex-auto">
          <h1 className="text-base font-semibold leading-6 text-white">Výpůjčky</h1>
          <p className="mt-2 text-sm text-gray-300">
            Seznam všech výpůjček knih
          </p>
        </div>
        <div className="mt-4 sm:ml-16 sm:mt-0 sm:flex-none">
          <button
            type="button"
            onClick={handleAdd}
            className="block rounded-md bg-indigo-600 px-3 py-2 text-center text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
          >
            Nová výpůjčka
          </button>
        </div>
      </div>

      {error && (
        <div className="mt-4 rounded-md bg-red-50 p-4">
          <div className="flex">
            <div className="ml-3">
              <h3 className="text-sm font-medium text-red-800">{error}</h3>
            </div>
          </div>
        </div>
      )}

      <div className="mt-8 flow-root">
        <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
            <table className="min-w-full divide-y divide-gray-700">
              <thead>
                <tr>
                  <th scope="col" className="py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-white sm:pl-0">
                    Kniha
                  </th>
                  <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-white">
                    Uživatel
                  </th>
                  <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-white">
                    Vypůjčeno
                  </th>
                  <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-white">
                    Vráceno
                  </th>
                  <th scope="col" className="relative py-3.5 pl-3 pr-4 sm:pr-0">
                    <span className="sr-only">Akce</span>
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-700">
                {lends.map((lend) => (
                  <tr key={lend.id}>
                    <td className="whitespace-nowrap py-4 pl-4 pr-3 text-sm font-medium text-white sm:pl-0">
                      {lend.book.name}
                    </td>
                    <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-300">{lend.user.name}</td>
                    <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-300">
                      {new Date(lend.lended).toLocaleDateString()}
                    </td>
                    <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-300">
                      {lend.returned ? new Date(lend.returned).toLocaleDateString() : '-'}
                    </td>
                    <td className="relative whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-medium sm:pr-0">
                      {!lend.returned && (
                        <button
                          onClick={() => handleReturn(lend.id)}
                          className="text-indigo-400 hover:text-indigo-300"
                        >
                          Vrátit
                        </button>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {isModalOpen && (
        <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity">
          <div className="fixed inset-0 z-10 overflow-y-auto">
            <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
              <div className="relative transform overflow-hidden rounded-lg bg-white px-4 pb-4 pt-5 text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg sm:p-6">
                <div>
                  <h3 className="text-base font-semibold leading-6 text-gray-900">
                    Nová výpůjčka
                  </h3>
                  <form onSubmit={handleSubmit(onSubmit)} className="mt-4 space-y-4">
                    <div>
                      <label htmlFor="bookId" className="block text-sm font-medium text-gray-700">
                        Kniha
                      </label>
                      <select
                        {...register('bookId')}
                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                      >
                        <option value="">Vyberte knihu</option>
                        {books.map((book) => (
                          <option key={book.id} value={book.id}>
                            {book.name} - {book.author} ({book.inStock} kusů)
                          </option>
                        ))}
                      </select>
                      {errors.bookId && (
                        <p className="mt-1 text-sm text-red-600">{errors.bookId.message}</p>
                      )}
                    </div>

                    <div>
                      <label htmlFor="userId" className="block text-sm font-medium text-gray-700">
                        Uživatel
                      </label>
                      <select
                        {...register('userId')}
                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                      >
                        <option value="">Vyberte uživatele</option>
                        {users.map((user) => (
                          <option key={user.id} value={user.id}>
                            {user.name}
                          </option>
                        ))}
                      </select>
                      {errors.userId && (
                        <p className="mt-1 text-sm text-red-600">{errors.userId.message}</p>
                      )}
                    </div>

                    <div className="mt-5 sm:mt-6 sm:grid sm:grid-flow-row-dense sm:grid-cols-2 sm:gap-3">
                      <button
                        type="submit"
                        className="inline-flex w-full justify-center rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 sm:col-start-2"
                      >
                        Vytvořit
                      </button>
                      <button
                        type="button"
                        onClick={() => {
                          setIsModalOpen(false)
                          reset()
                        }}
                        className="mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 sm:col-start-1 sm:mt-0"
                      >
                        Zrušit
                      </button>
                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  )
} 