// @ts-check
const { test, expect } = require('@playwright/test');

test('Add a expense', async ({ page }) => {
  await page.goto('http://127.0.0.1:5500/');

    const addAmount = page.locator('#overlay');
    const selectCategory = page.locator('#expenseCategory');
    const addName = page.locator('#objectName')
    
    // Enter expense details
    await addAmount.fill('50')
    await selectCategory.selectOption('Food')
    await addName.fill('Ice cream')

    // Submit the form
    await page.click('button[type="submit"]');

  // Expect to see the new expense in the expenses list
    let tracker = await page.locator('.transactionThisMonth')
    let trackerText = await tracker.textContent();
    
    await expect(trackerText).toEqual('Ice cream: 50 Kr (food) ');
});

test('Total Amount', async ({ page }) => {
    await page.goto('http://127.0.0.1:5500/');

    const addAmount = page.locator('#overlay');
    const selectCategory = page.locator('#expenseCategory');
    const addName = page.locator('#objectName')
    
    // Enter expense details
    await addAmount.fill('100')
    await selectCategory.selectOption('Salary')
    await addName.fill('Swish')

    // Submit the form
    await page.click('button[type="submit"]');
    // repeat with diferent values
    await addAmount.fill('50')
    await selectCategory.selectOption('Food')
    await addName.fill('Ice cream')

    await page.click('button[type="submit"]');

    let tracker = await page.locator('#totalAmount')
    let trackerText = await tracker.textContent();

    await expect(trackerText).toEqual('Total amount: 50.00');
});

test('Delete transaction', async ({ page }) => {
    await page.goto('http://127.0.0.1:5500/');

    const addAmount = page.locator('#overlay');
    const selectCategory = page.locator('#expenseCategory');
    const addName = page.locator('#objectName')
    
    // Enter expense details
    await addAmount.fill('100')
    await selectCategory.selectOption('Salary')
    await addName.fill('Swish')

    // Submit the form
    await page.click('button[type="submit"]');

    // repeat with diferent values
    await addAmount.fill('50')
    await selectCategory.selectOption('Food')
    await addName.fill('Ice cream')

    await page.click('button[type="submit"]');

    //go to history
    await page.click('.expandButton');
    //select the month
    const selectMonth = page.locator('#expenseMonth');
    await selectMonth.selectOption('Mar');

    const deleteButtons = await page.$$('#deleteButton');
    const lastDeleteButton = deleteButtons[1];
    await lastDeleteButton.click();

    await page.click('.closeButton')

    let tracker = await page.locator('#totalAmount')
    let trackerText = await tracker.textContent();

    await expect(trackerText).toEqual('Total amount: 100.00');
});