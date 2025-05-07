import { NextResponse } from 'next/server'

export async function PUT(
  request: Request,
  { params }: { params: { id: string } }
) {
  try {
    const body = await request.json()
    const response = await fetch(`http://localhost:5000/api/books/${params.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    })

    if (!response.ok) {
      const error = await response.json()
      return NextResponse.json(
        { message: error.message || 'Chyba při aktualizaci knihy' },
        { status: 400 }
      )
    }

    const data = await response.json()
    return NextResponse.json(data)
  } catch (error) {
    console.error('Error updating book:', error)
    return NextResponse.json(
      { message: 'Chyba při aktualizaci knihy' },
      { status: 500 }
    )
  }
} 